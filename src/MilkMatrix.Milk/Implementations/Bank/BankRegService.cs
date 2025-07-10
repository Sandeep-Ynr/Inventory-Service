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
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Milk.Models.Queries.BankQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class BankRegService :IBankRegService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;
        public BankRegService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(HamletService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddBankReg(BankRegInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "RegionalCode", request.RegionalCode ?? (object)DBNull.Value },
                    { "BankID", request.BankID ?? (object)DBNull.Value },
                    { "RegionalBankName",  request.RegionalBankName ?? (object)DBNull.Value },
                    { "RegionalBankShortName",  request.RegionalBankShortName?? (object)DBNull.Value  },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },

                };
                var response = await repository.AddAsync(BankRelgionQueries.AddBankRelgion, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Hamlet {request.RegionalBankName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Hamlet: {request.RegionalBankName}", ex);
                throw;
            }
        }

        public async Task UpdateBankReg(BankRegUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RegionalID", request.RegionalID },
                    { "RegionalCode", request.RegionalCode },
                    { "RegionalBankName", request.RegionalBankName ?? (object)DBNull.Value },
                    { "RegionalBankShortName", request.RegionalBankShortName ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
                    //{ "IsStatus", request.IsActive ?? (object)DBNull.Value }
                };
                await repository.UpdateAsync(BankRelgionQueries.AddBankRelgion, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Bank Regional {request.RegionalBankName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Bank Regional: {request.RegionalBankName}", ex);
                throw;
            }
        }
        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "ModifyBy",userId },
                    { "RegionalID", id}
                };

                await repository.DeleteAsync(BankRelgionQueries.AddBankRelgion, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Bank Regional with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Bank Regional ID: {id}", ex);
                throw;
            }
        }

        public async Task<BankRegResponse?> GetById(int id)
        {

            try
            {
                logging.LogInfo($"GetByIdAsync called for Tehsil id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BankRegResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<BankRegResponse>(BankRelgionQueries.GetBankRelgion, new Dictionary<string, object>
                {
                    { "ActionType", 2 },
                    { "RegionalID", id },
                    { "IsStatus", true }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new BankRegResponse();
                logging.LogInfo(result != null
                    ? $"Bank Regional with id {id} retrieved successfully."
                    : $"Bank Regional with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
        }
        public async Task<IEnumerable<BankRegResponse>> GetBankReg(BankRegionalRequest request)
        {
            var repository = repositoryFactory.Connect<BankRegResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"RegionalID", request.BankRegionalId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<BankRegResponse>(BankRelgionQueries.GetBankRelgion, requestParams, null, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(BankRegionalRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)request.ActionType },
                { "RegionalID", request.BankRegionalId  },
            };
            var response = await repository.QueryAsync<CommonLists>(
                BankRelgionQueries.GetBankRelgion, requestParams, null, CommandType.StoredProcedure
            );
            return response;
        }

        public async Task<IListsResponse<BankRegResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BankRegResponse, int, FiltersMeta>(BankRelgionQueries.GetBankRelgionList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<BankRegResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
