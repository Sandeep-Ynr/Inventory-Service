using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Milk.Contracts.Member;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Response.Member;
using MilkMatrix.Milk.Models.Queries;


namespace MilkMatrix.Milk.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MemberService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MemberService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task<MemberResponse?> AddMember(MemberInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                 {
                     {"ActionType", (int)CrudActionType.Create},
                     {"MemberCode", request.MemberCode?? (object)DBNull.Value},
                     {"FarmerName", request.FarmerName?? (object)DBNull.Value},
                     {"LocalName", request.LocalName ?? (object)DBNull.Value},
                     {"Gender", request.Gender?? (object)DBNull.Value},
                     {"DateOfBirth", request.DateOfBirth},
                     {"MobileNo", request.MobileNo?? (object)DBNull.Value},
                     {"AlternateNo", request.AlternateNo?? (object)DBNull.Value},
                     {"EmailID", request.EmailID ?? (object)DBNull.Value},
                     {"AadharNo", request.AadharNo ?? (object)DBNull.Value},
                     {"SocietyID", request.SocietyID},
                     {"CreatedBy", request.CreatedBy},
                     {"BusinessID", request.BusinessID},
                 };
                var message = await repository.AddAsync(MemberQueries.AddOrUpdateMember, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member {request.MemberCode} added successfully with response: {message}");

                var obj = new MemberResponse
                {
                    MemberID = Convert.ToInt32( message) // Assuming your SP returns MemberCode
                };

                return obj;
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddMember", ex);
                throw;
            }
        }

        public async Task UpdateMember(MemberUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    {"MemberID", request.MemberID ?? (object)DBNull.Value},
                    {"MemberCode", request.MemberCode ?? (object)DBNull.Value},
                    {"FarmerName", request.FarmerName ?? (object)DBNull.Value},
                    {"LocalName", request.LocalName ?? (object)DBNull.Value},
                    {"Gender", request.Gender ?? (object)DBNull.Value},
                    {"DateOfBirth", request.DateOfBirth},
                    {"MobileNo", request.MobileNo ?? (object)DBNull.Value},
                    {"AlternateNo", request.AlternateNo ?? (object)DBNull.Value},
                    {"EmailID", request.EmailID ?? (object)DBNull.Value},
                    {"AadharNo", request.AadharNo ?? (object)DBNull.Value},
                    {"SocietyID", request.SocietyID},
                    {"IsStatus", request.IsActive ?? (object)DBNull.Value},
                    { "ModifyBy", request.ModifiedBy?? (object)DBNull.Value }
                };

                var message = await repository.UpdateAsync(MemberQueries.AddOrUpdateMember, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member {request.MemberCode} updated successfully with response: {message}");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in UpdateMember", ex);
                throw;
            }
        }

        public async Task Delete(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Delete},
                    {"MemberID", id},
                    {"ModifyBy", userId}
                };
                var response = await repository.DeleteAsync(MemberQueries.AddOrUpdateMember, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Member with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Delete for Member ID: {id}", ex);
                throw;
            }
        }
        public async Task<MemberResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetById called for Member ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MemberResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MemberResponse>("usp_member_list", new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.Individual},
                    {"MemberID", id}
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetById", ex);
                throw;
            }
        }
        public async Task<IEnumerable<MemberResponse>> GetMembers(MemberRequestModel request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<MemberResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)(request.ActionType ?? ReadActionType.All)},
                    {"MemberID", request.MemberID ?? (object)DBNull.Value},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value}
                };

                return await repo.QueryAsync<MemberResponse>(MemberQueries.GetMemberList, parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMembers", ex);
                throw;
            }
        }
        public async Task<IListsResponse<MemberResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MemberResponse, int, FiltersMeta>(MemberQueries.GetMemberList,
                    DbConstants.Main, parameters, null);
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<MemberResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

    }
}
