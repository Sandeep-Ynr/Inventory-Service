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
    public class AccountLedgerService : IAccountLedgerService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public AccountLedgerService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(AccountLedgerService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task InsertAccountLedger(AccountHeadsInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create },    // 1 = Insert, 2 = Update, 3 = Delete
                        { "BusinessId", request.BusinessId },
                        { "Code", request.Code },
                        { "Name", request.Name ?? (object)DBNull.Value },
                        { "GroupId", request.GroupId },
                        { "LedgerType", request.LedgerType },            // E=Expense, I=Income, L=Liability, A=Asset
                        { "CashBankType", request.CashBankType },        // O=Other, B=Bank, C=Cash
                        { "CityId", request.CityId ?? (object)DBNull.Value },
                        { "CityText", request.CityText ?? (object)DBNull.Value },
                        { "AlternateCode", request.AlternateCode ?? (object)DBNull.Value },
                        { "BudgetApplicable", request.BudgetApplicable },
                        { "CostCenterApplicable", request.CostCenterApplicable },
                        { "TdsApplicable", request.TdsApplicable },
                        { "IsActive", request.IsActive },
                        { "BranchId", request.BranchId ?? (object)DBNull.Value },
                        { "Notes", request.Notes ?? (object)DBNull.Value },
                        { "CreatedBy", request.CreatedBy  }
                    };


                var message = await repository.AddAsync(AccountsQueries.InsupdAccountHeads, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Ledger  {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Insert/Update Ledger: {request.Name}", ex);
                throw;
            }

        }

        public async Task UpdateAccountLedger(int id, AccountHeadsUpdateRequest request)

        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
          {
                        { "ActionType", (int)CrudActionType.Update },
                        { "id", id },
                        { "BusinessId", request.BusinessId },
                        { "Code", request.Code },
                        { "Name", request.Name ?? (object)DBNull.Value },
                        { "GroupId", request.GroupId },
                        { "LedgerType", request.LedgerType },            // E=Expense, I=Income, L=Liability, A=Asset
                        { "CashBankType", request.CashBankType },        // O=Other, B=Bank, C=Cash
                        { "CityId", request.CityId ?? (object)DBNull.Value },
                        { "CityText", request.CityText ?? (object)DBNull.Value },
                        { "AlternateCode", request.AlternateCode ?? (object)DBNull.Value },
                        { "BudgetApplicable", request.BudgetApplicable },
                        { "CostCenterApplicable", request.CostCenterApplicable },
                        { "TdsApplicable", request.TdsApplicable },
                        { "IsActive", request.IsActive },
                        { "BranchId", request.BranchId ?? (object)DBNull.Value },
                        { "Notes", request.Notes ?? (object)DBNull.Value },
                        { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }

          };

                var message = await repository.UpdateAsync(AccountsQueries.InsupdAccountHeads, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Heads {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Update Account Heads: {request.Name}", ex);
                throw;
            }

        }


        //public async Task DeleteAccountGroupById(int id, int userId)
        //{
        //    try
        //    {
        //        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
        //        var requestParams = new Dictionary<string, object>
        //        {
        //            {"ActionType" , (int)CrudActionType.Delete },
        //            {"id", id },
        //            {"Is_Active", false },
        //            {"ModifyBy", userId }

        //        };

        //        var response = await repository.DeleteAsync(AccountsQueries.InsupdAccountGroup, requestParams, CommandType.StoredProcedure);


        //        if (response.StartsWith("Error"))
        //        {
        //            throw new Exception($"Stored Procedure Error: {response}");
        //        }
        //        else
        //        {
        //            logging.LogInfo($"Account Group {response} deleted successfully.");
        //        }

        //        logging.LogInfo($"Account Group with id {id} deleted successfully.");

        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in DeleteAsync for Account Group  id: {id}", ex);
        //        throw  new Exception(ex.Message);
        //    }

        //}




        public async Task<IListsResponse<AccountHeadsResponse>> GetAccountHeadList(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<AccountHeadsResponse, int, FiltersMeta>(AccountsQueries.GetAccountHeadsList,
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
            return new ListsResponse<AccountHeadsResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<AccountHeadsResponse?> GetAccountHeadById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Account Ledger id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<AccountHeadsResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<AccountHeadsResponse>(AccountsQueries.GetAccountHeadsList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "Id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new AccountHeadsResponse();
                logging.LogInfo(result != null
                    ? $"Account Ledger with id {id} retrieved successfully."
                    : $"Account Ledger with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Account Ledger: {id}", ex);
                throw;
            }
        }


        public async Task DeleteAccountHeadById(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"id", id },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(AccountsQueries.InsupdAccountHeads, requestParams, CommandType.StoredProcedure);


                if (response.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {response}");
                }
                else
                {
                    logging.LogInfo($"Account Head {response} deleted successfully.");
                }

                logging.LogInfo($"Account Head with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Account Head  id: {id}", ex);
                throw new Exception(ex.Message);
            }

        }




    }
}
