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
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class TehsilService : ITehsilService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public TehsilService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(TehsilService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(TehsilQueries.GetTehsil, requestParams, null, CommandType.StoredProcedure);

            return response;

        }

        public async Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<TehsilResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<TehsilResponse>(TehsilQueries.GetTehsil, requestParams, null, CommandType.StoredProcedure);

            return response;
        }

        public async Task<TehsilResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Tehsil id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<TehsilResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<TehsilResponse>(TehsilQueries.GetTehsil, new Dictionary<string, object> 
                { 
                    { "ActionType", 2 },
                    { "TehsilId", id },
                    { "IsStatus", true }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new TehsilResponse();
                logging.LogInfo(result != null
                    ? $"Tehsil with id {id} retrieved successfully."
                    : $"Tehsil with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Tehsil id: {id}", ex);
                throw;
            }
        }
        public async Task AddTehsilAsync(TehsilInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create},
                    { "TehsilName", request.TehsilName ?? (object)DBNull.Value },
                    { "DistrictId", request.DistrictId ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy }
                    
                };

                var response = await repository.AddAsync(TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Tehsil {request.TehsilName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Tehsil: {request.TehsilName}", ex);
                throw;
            }
        }

        public async Task UpdateTehsilAsync(TehsilUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "TehsilId", request.TehsilId},
                    { "TehsilName", request.TehsilName ?? (object)DBNull.Value },
                    { "DistrictId", request.DistrictId},
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                var response = await repository.UpdateAsync(
                   TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Tehsil {request.TehsilName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Tehsil: {request.TehsilName}", ex);
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
                    {"TehsilId", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };

                var response = await repository.DeleteAsync(TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Tehsil with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Tehsil id: {id}", ex);
                throw;
            }

        }
        public async Task<IListsResponse<TehsilResponse>> GetAllAsync(IListsRequest request, int userId)
        {
            var user = await GetByIdAsync(userId);
            var parameters = new Dictionary<string, object>()
            {
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<TehsilResponse, int, FiltersMeta>(TehsilQueries.GetTehsilList,
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
            return new ListsResponse<TehsilResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }


    }
}
