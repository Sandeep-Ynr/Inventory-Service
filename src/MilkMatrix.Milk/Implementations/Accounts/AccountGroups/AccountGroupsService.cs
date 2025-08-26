using System.Data;
using Azure.Core;
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
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Accounts.AccountGroups;
using MilkMatrix.Milk.Contracts.Admin.GlobleSetting;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;
using MilkMatrix.Milk.Models.Response.Accounts.AccountGroups;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Milk.Models.Queries.AccountsQueries;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.BmcQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class AccountGroupsService : IAccountGroupsService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public AccountGroupsService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(AccountGroupsService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task InsertAccountGroup(AccountGroupsInsertRequest request)


        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "businessId", request.BusinessId },
                    { "Name", request.Name ??(object) DBNull.Value },
                    { "code", request.Code },
                    { "parentId", request.ParentId ?? (object)DBNull.Value },
                    { "scheduleId", request.ScheduleId },
                    { "allowPosting", request.AllowPosting },
                    { "notes", request.Notes ?? (object)DBNull.Value  },
                    { "createdBy", request.CreatedBy ?? (object)DBNull.Value }

                };

                var message = await repository.AddAsync(AccountsQueries.InsupdAccountGroup, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Group {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Insert/Update Sequence: {request.Name}", ex);
                throw;
            }

        }

        public async Task UpdateAccountGroup( int id, AccountGroupsUpdateRequest request)


        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
          {
              { "ActionType", (int)CrudActionType.Update },
              { "Id", id },
              { "businessId", request.BusinessId },
              { "Name", request.Name ??(object) DBNull.Value },
              { "code", request.Code },
              { "is_active", request.IsActive },
              { "parentId", request.ParentId ?? (object)DBNull.Value },
              { "scheduleId", request.ScheduleId },
              { "allowPosting", request.AllowPosting },
              { "notes", request.Notes ?? (object)DBNull.Value  },
              { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }

          };

                var message = await repository.UpdateAsync(AccountsQueries.InsupdAccountGroup, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Group {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Update Account Group: {request.Name}", ex);
                throw;
            }

        }


        public async Task DeleteAccountGroupById(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"id", id },
                    {"Is_Active", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(AccountsQueries.InsupdAccountGroup, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Account Group with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Account Group  id: {id}", ex);
                throw;
            }

        }



        //public Task DeleteSequance(int id, int userId)
        //{
        //    throw new NotImplementedException();
        //}



        public async Task<IListsResponse<AccountGroupsResponse>> GetAccountGroupList(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<AccountGroupsResponse, int, FiltersMeta>(AccountsQueries.GetAccountGroupsList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<AccountGroupsResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<AccountGroupsResponse?> GetAccountGroupById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Account Group id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<AccountGroupsResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<AccountGroupsResponse>(AccountsQueries.GetAccountGroupsList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "Id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new AccountGroupsResponse();
                logging.LogInfo(result != null
                    ? $"Account Group with id {id} retrieved successfully."
                    : $"Account Group with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Account Group: {id}", ex);
                throw;
            }
        }

        //public  async Task<AccountGroupsResponse?> GetSequanceById(string HeadName)
        //{
        //    try
        //    {
        //        logging.LogInfo($"GetByIdAsync called for Get Sequance id: {HeadName}");
        //        var repo = repositoryFactory
        //                   .ConnectDapper<AccountGroupsResponse>(DbConstants.Main);
        //        var data = await repo.QueryAsync<AccountGroupsResponse>(SequenceQuery.GetSequenceList, new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)ReadActionType.Individual },
        //            { "HeadName", HeadName }
        //        }, null);

        //        var result = data.Any() ? data.FirstOrDefault() : new AccountGroupsResponse();
        //        logging.LogInfo(result != null
        //            ? $"Sequance with id {HeadName} retrieved successfully."
        //            : $"Sequance with id {HeadName} not found.");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in GetByIdAsync for Sequance id: {HeadName}", ex);
        //        throw;
        //    }
        //}


    }
}
