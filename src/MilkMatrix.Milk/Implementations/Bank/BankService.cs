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
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class BankService : IBankService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public BankService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddBank(BankInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },  // 1 = Insert
                    { "BankCode", request.BankCode ?? (object)DBNull.Value },
                    { "BankName", request.BankName ?? (object)DBNull.Value },
                    { "BankShortName", request.BankShortName ?? (object)DBNull.Value },
                    { "country_id", request.CountryId ?? (object)DBNull.Value },
                    { "state_id", request.StateId ?? (object)DBNull.Value },
                    { "district_id", request.DistrictId ?? (object)DBNull.Value },
                    { "AccountNoLength", request.AccountNoLength ?? (object)DBNull.Value },
                    { "BankTypeID", request.BankTypeID ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "IsAccountValidationEnabled", request.IsAccountValidationEnabled ?? (object)DBNull.Value },
                    { "IsNationalized", request.IsNationalized ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value }
                };

                var response = await repository.AddAsync(BankMasterQueries.AddBank, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Bank {request.BankName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Bank: {request.BankName}", ex);
                throw;
            }
        }

        public async Task UpdateBank(BankUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "BankId", request.BankID },
                    { "BankTypeId", request.BankTypeID ?? (object)DBNull.Value},
                    { "BankCode", request.BankCode ?? (object)DBNull.Value },
                    { "BankName", request.BankName ?? (object)DBNull.Value },
                    { "BankShortName", request.BankShortName ?? (object)DBNull.Value },
                    { "Country_id", request.CountryId ?? (object)DBNull.Value },
                    { "state_id", request.StateId ?? (object)DBNull.Value },
                    { "district_id", request.DistrictId ?? (object)DBNull.Value },
                    { "AccountNoLength", request.AccountNoLength ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "IsAccountValidationEnabled", request.IsAccountValidationEnabled ?? (object)DBNull.Value },
                    { "IsNationalized", request.IsNationalized ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
                };
                await repository.UpdateAsync(BankMasterQueries.AddBank, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Bank {request.BankName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Bank: {request.BankName}", ex);
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
                    { "ActionType", (int)CrudActionType.Delete },
                    { "BankId", id },
                    { "ModifyBy", userId }
                };
                var response = await repository.DeleteAsync(BankMasterQueries.AddBank, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Bank with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Bank ID: {id}", ex);
                throw;
            }
        }

        public async Task<BankResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetById called for Bank Type id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BankResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<BankResponse>(BankMasterQueries.GetBankList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "BankId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new BankResponse();
                logging.LogInfo(result != null
                    ? $"Bank with id {id} retrieved successfully."
                    : $"Bank with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for Bank id: {id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<BankResponse>> GetBank(BankRequest request)
        {
            var repository = repositoryFactory.Connect<BankResponse>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
                {
                    { "BankId", request.BankID },
                    { "ActionType", (int)request.ActionType },
                    { "IsStatus", request.IsActive }
                };

            var response = await repository.QueryAsync<BankResponse>(BankMasterQueries.GetBankList, requestParams, null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<IListsResponse<BankResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BankResponse, int, FiltersMeta>(BankMasterQueries.GetBankList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<BankResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(BankRequest request)
        {
            var repository = repositoryFactory.Connect<BankResponse>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)request.ActionType },
                    { "IsStatus", request.IsActive }
                };
            var response = await repository.QueryAsync<CommonLists>(BankMasterQueries.GetBankList, requestParams, null, CommandType.StoredProcedure);
            return response;
        }
    }
}
