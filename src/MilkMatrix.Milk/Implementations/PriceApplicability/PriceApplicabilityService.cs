using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Dtos;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.PriceApplicability;
using MilkMatrix.Milk.Models.Request.PriceApplicability;
using MilkMatrix.Milk.Models.Response.PriceApplicability;
using static MilkMatrix.Milk.Models.Queries.PriceApplicabilityQueries;

namespace MilkMatrix.Milk.Implementations.PriceApplicability
{
    public class PriceApplicabilityService : IPriceApplicabilityService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public PriceApplicabilityService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(PriceApplicabilityService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddAsync(PriceAppInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "RateCode", request.RateCode ?? (object)DBNull.Value },
                    { "ModuleCode", request.ModuleCode ?? (object)DBNull.Value },
                    { "ModuleName", request.ModuleName ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "RateFor", request.RateFor ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(PriceApplicabilityQuery.AddPriceApp, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Price Applicability {request.ModuleName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Price Applicability: {request.ModuleName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(PriceAppUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RateAppId", request.RateAppId},
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "RateCode", request.RateCode ?? (object)DBNull.Value },
                    { "ModuleCode", request.ModuleCode ?? (object)DBNull.Value },
                    { "ModuleName", request.ModuleName ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "RateFor", request.RateFor ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(PriceApplicabilityQuery.AddPriceApp, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Price Applicability {request.ModuleName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Price Applicability: {request.ModuleName}", ex);
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
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"RateAppId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(PriceApplicabilityQuery.AddPriceApp, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Price Applicability with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Price Applicability id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<PriceAppInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<PriceAppInsertResponse, int, FiltersMeta>(PriceApplicabilityQuery.GetPriceAppList,
                    DbConstants.Main, parameters, null);

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
            return new ListsResponse<PriceAppInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<PriceAppInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for BMC id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<PriceAppInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<PriceAppInsertResponse>(PriceApplicabilityQuery.GetPriceAppList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RateAppId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new PriceAppInsertResponse();
                logging.LogInfo(result != null
                    ? $"Price Applicability with id {id} retrieved successfully."
                    : $"Price Applicability with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Price Applicability id: {id}", ex);
                throw;
            }
        }

    }
}
