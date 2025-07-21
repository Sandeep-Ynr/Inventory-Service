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
using MilkMatrix.Milk.Contracts.Milk;
using MilkMatrix.Milk.Implementations.Milk;
using MilkMatrix.Milk.Models.Request.Milk;
using MilkMatrix.Milk.Models.Response.Animal;

//using MilkMatrix.Milk.Models.Response.Animal;
using MilkMatrix.Milk.Models.Response.Milk;
using static MilkMatrix.Milk.Models.Queries.AnimalQueries;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;

namespace MilkMatrix.Milk.Implementations.Milk
{
    public class MilkService : IMilkService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MilkService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MilkService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }


        public async Task AddAsync(MilkTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "MilkTypeName",request.MilkTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(MilkTypeQueries.InsupdMilkType, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Milk Type {request.MilkTypeName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Milk Type: {request.MilkTypeName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(MilkTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "MilkTypeId", request.MilkTypeId},
                    { "MilkTypeName",request.MilkTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(MilkTypeQueries.InsupdMilkType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Milk Type {request.MilkTypeName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Milk Type: {request.MilkTypeName}", ex);
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
                    {"MilkTypeId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(MilkTypeQueries.InsupdMilkType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Milk Type with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Milk Type id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<MilkTypeInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MilkTypeInsertResponse, int, FiltersMeta>(MilkTypeQueries.GetMilkTypeList,
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
            return new ListsResponse<MilkTypeInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<MilkTypeInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Milk Type id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<MilkTypeInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MilkTypeInsertResponse>(MilkTypeQueries.GetMilkTypeList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "MilkTypeId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new MilkTypeInsertResponse();
                logging.LogInfo(result != null
                    ? $"Milk Type with id {id} retrieved successfully."
                    : $"Milk Type with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Milk Type id: {id}", ex);
                throw;
            }
        }

        public async Task AddRateTypeAsync(RateTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "RateTypeName",request.RateTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(RateTypeQueries.InsupdRateType, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Rate Type {request.RateTypeName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Rate Type: {request.RateTypeName}", ex);
                throw;
            }
        }

        public async Task UpdateRateTypeAsync(RateTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RateTypeId", request.RateTypeId},
                    { "RateTypeName",request.RateTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(RateTypeQueries.InsupdRateType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Rate Type {request.RateTypeName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Rate Type: {request.RateTypeName}", ex);
                throw;
            }
        }

        public async Task DeleteRateTypeAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"RateTypeId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(RateTypeQueries.InsupdRateType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Rate Type with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Rate Type id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<RateTypeInsertResponse>> GetAllRateTypeAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<RateTypeInsertResponse, int, FiltersMeta>(RateTypeQueries.GetRateTypeList,
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
            return new ListsResponse<RateTypeInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<RateTypeInsertResponse?> GetRateTypeByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Milk Type id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<RateTypeInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<RateTypeInsertResponse>(RateTypeQueries.GetRateTypeList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RateTypeId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new RateTypeInsertResponse();
                logging.LogInfo(result != null
                    ? $"Rate Type with id {id} retrieved successfully."
                    : $"Rate Type with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Rate Type id: {id}", ex);
                throw;
            }
        }

    }
}
