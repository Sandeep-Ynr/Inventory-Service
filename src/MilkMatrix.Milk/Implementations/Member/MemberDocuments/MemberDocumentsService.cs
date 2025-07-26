using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Milk.Contracts.Member.MemberDocuments;
using MilkMatrix.Milk.Models.Request.Member.MemberDocuments;
using MilkMatrix.Milk.Models.Response.Member.MemberDocuments;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.DataProvider;
using System.Data;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using System.Linq;

namespace MilkMatrix.Milk.Implementations.Member.MemberDocuments
{
    public class MemberDocumentsService : IMemberDocumentsService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MemberDocumentsService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MemberDocumentsService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddMemberDocuments(MemberDocumentsInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Create},
                    {"MemberID", request.MemberID},
                    {"AadharFile", request.AadharFile ?? (object)DBNull.Value},
                    {"VoterOrRationCard", request.VoterOrRationCard ?? (object)DBNull.Value},
                    {"OtherDocument", request.OtherDocument ?? (object)DBNull.Value},
                    {"is_status", request.IsStatus},
                    {"created_on", DateTime.UtcNow}, // Using UtcNow for created_on
                    {"created_by", request.CreatedBy},
                    {"is_deleted", false} // Assuming is_deleted is initially false on creation
                };

                var message = await repository.AddAsync(MemberQueries.AddOrUpdateMemberDocuments, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Documents for Member ID {request.MemberID} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddMemberDocuments", ex);
                throw;
            }
        }

        public async Task UpdateMemberDocuments(MemberDocumentsUpdateRequest request)
        {
             try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    {"DocumentID", request.DocumentID},
                    {"MemberID", request.MemberID},
                    {"AadharFile", request.AadharFile ?? (object)DBNull.Value},
                    {"VoterOrRationCard", request.VoterOrRationCard ?? (object)DBNull.Value},
                    {"OtherDocument", request.OtherDocument ?? (object)DBNull.Value},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"ModifyOn", request.ModifiedOn ?? (object)DBNull.Value},
                    {"ModifyBy", request.ModifiedBy ?? (object)DBNull.Value},
                    {"IsDeleted", request.IsDeleted ?? (object)DBNull.Value}
                };

                var message = await repository.UpdateAsync(MemberQueries.AddOrUpdateMemberDocuments, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Documents with ID {request.DocumentID} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in UpdateMemberDocuments", ex);
                throw;
            }
        }

        public async Task DeleteMemberDocuments(long documentId, long userId)
        {
             try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Delete},
                    {"DocumentID", documentId},
                    {"modify_on", DateTime.UtcNow}, // Using UtcNow for modify_on
                    {"modify_by", userId}
                };

                var response = await repository.DeleteAsync(MemberQueries.DeleteMemberDocuments, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Member Documents with ID {documentId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteMemberDocuments for Document ID: {documentId}", ex);
                throw;
            }
        }

        public async Task<MemberDocumentsResponse?> GetMemberDocumentsById(long id)
        {
             try
            {
                logging.LogInfo($"GetById called for MemberDocuments ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MemberDocumentsResponse>(DbConstants.Main); // Assuming Dapper and MemberDocumentsResponse are appropriate
                var data = await repo.QueryAsync<MemberDocumentsResponse>(MemberQueries.GetMemberDocumentsById, new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.Individual},
                    {"DocumentID", id}
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberDocumentsById", ex);
                throw;
            }
        }

        public async Task<IEnumerable<MemberDocumentsResponse>> GetMemberDocumentsByMemberId(long memberId)
        {
            try
            {
                logging.LogInfo($"GetMemberDocumentsByMemberId called for MemberID: {memberId}");
                var repo = repositoryFactory.ConnectDapper<MemberDocumentsResponse>(DbConstants.Main); // Assuming Dapper and MemberDocumentsResponse are appropriate
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.SpecificFields}, // Assuming you have a ReadActionType.ByMember enum
                    {"MemberID", memberId}
                };

                return await repo.QueryAsync<MemberDocumentsResponse>(MemberQueries.GetMemberDocumentsList, parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetMemberDocumentsByMemberId for MemberID: {memberId}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MemberDocumentsResponse>> GetAllMemberDocuments(IListsRequest request)
        {
            try
            {
                var parameters = new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.All }
                };

                var (allResults, countResult, filterMetas) = await queryMultipleData
                    .GetMultiDetailsAsync<MemberDocumentsResponse, int, FiltersMeta>(MemberQueries.GetMemberDocumentsList,
                        DbConstants.Main, parameters, null);
                var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
                var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
                var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

                var filtered = allResults.AsQueryable().ApplyFilters(filters);
                var sorted = filtered.ApplySorting(sorts);
                var paged = sorted.ApplyPaging(paging);
                var filteredCount = filtered.Count();

                return new ListsResponse<MemberDocumentsResponse>
                {
                    Count = filteredCount,
                    Results = paged.ToList(),
                    Filters = filterMetas
                };
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetAllMemberDocuments", ex);
                throw;
            }
        }
    }
}
