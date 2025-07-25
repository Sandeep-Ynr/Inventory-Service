using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Milk.Contracts.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Response.Member.MemberBankDetails;
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

namespace MilkMatrix.Milk.Implementations.Member.MemberBankDetails
{
    public class MemberBankDetailsService : IMemberBankDetailsService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MemberBankDetailsService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MemberBankDetailsService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddMemberBankDetails(MemberBankDetailsInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Create},
                    {"MemberID", request.MemberID},
                    {"BankID", request.BankID},
                    {"BranchID", request.BranchID},
                    {"AccountHolderName", request.AccountHolderName},
                    {"AccountNumber", request.AccountNumber},
                    {"IFSCCode", request.IFSCCode},
                    {"IsJointAccount", request.IsJointAccount},
                    {"PassbookFilePath", request.PassbookFilePath ?? (object)DBNull.Value},
                    { "IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"CreatedBy", request.CreatedBy ?? (object)DBNull.Value}
                };
          
                var message = await repository.AddAsync(MemberQueries.AddOrUpdateMemberBankDetails, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Bank Details for Member ID {request.MemberID} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddMemberBankDetails", ex);
                throw;
            }
        }

        public async Task UpdateMemberBankDetails(MemberBankDetailsUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    {"BankDetailID", request.BankDetailID},
                    {"MemberID", request.MemberID},
                    {"BankID", request.BankID},
                    {"BranchID", request.BranchID},
                    {"AccountHolderName", request.AccountHolderName},
                    {"AccountNumber", request.AccountNumber},
                    {"IFSCCode", request.IFSCCode},
                    {"IsJointAccount", request.IsJointAccount},
                    {"PassbookFilePath", request.PassbookFilePath ?? (object)DBNull.Value},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"ModifyBy", request.ModifiedBy ?? (object)DBNull.Value},
                };


                var message = await repository.UpdateAsync(MemberQueries.AddOrUpdateMemberBankDetails, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Bank Details with ID {request.BankDetailID} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in UpdateMemberBankDetails", ex);
                throw;
            }
        }

        public async Task DeleteMemberBankDetails(long bankDetailId, long userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Delete},
                    {"BankDetailID", bankDetailId},
                };

                var response = await repository.DeleteAsync(MemberQueries.AddOrUpdateMemberBankDetails, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Member Bank Details with ID {bankDetailId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteMemberBankDetails for Bank Detail ID: {bankDetailId}", ex);
                throw;
            }
        }

        public async Task<MemberBankDetailsResponse?> GetMemberBankDetailsById(long id)
        {
            try
            {
                logging.LogInfo($"GetById called for MemberBankDetails ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MemberBankDetailsResponse>(DbConstants.Main); // Assuming Dapper and MemberBankDetailsResponse are appropriate
                var data = await repo.QueryAsync<MemberBankDetailsResponse>(MemberQueries.GetMemberBankDetailsList, new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.Individual},
                    {"BankDetailID", id}
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberBankDetailsById", ex);
                throw;
            }
        }

        public async Task<IEnumerable<MemberBankDetailsResponse>> GetMemberBankDetails(MemberBankDetailsRequestModel request)
        {
             try
            {
                var repo = repositoryFactory.ConnectDapper<MemberBankDetailsResponse>(DbConstants.Main); // Assuming Dapper and MemberBankDetailsResponse are appropriate
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)(request.ActionType ?? ReadActionType.All)},
                    {"BankDetailID", request.BankDetailID ?? (object)DBNull.Value},
                    {"MemberID", request.MemberID ?? (object)DBNull.Value},
                    {"BankID", request.BankID ?? (object)DBNull.Value},
                    {"BranchID", request.BranchID ?? (object)DBNull.Value},
                    {"is_status", request.is_status ?? (object)DBNull.Value},
                    {"is_deleted", request.is_deleted ?? (object)DBNull.Value}
                };

                return await repo.QueryAsync<MemberBankDetailsResponse>(MemberQueries.GetMemberBankDetailsList, parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberBankDetails", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MemberBankDetailsResponse>> GetAllMemberBankDetails(IListsRequest request)
        {
             var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MemberBankDetailsResponse, int, FiltersMeta>(MemberQueries.GetMemberBankDetailsList,
                    DbConstants.Main, parameters, null);
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<MemberBankDetailsResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
