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
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;


namespace MilkMatrix.Milk.Implementations
{
    public class DistrictService : IDistrictService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public DistrictService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(DistrictRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(DistrictQueries.GetDistrict, requestParams, null, CommandType.StoredProcedure);

            return response;

        }
        public async Task AddDistrictsAsync(DistrictInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                { "DistrictName", request.DistrictName ?? (object)DBNull.Value }, 
                { "StateId", request.StateId ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
            };

                var response = await repository.AddAsync(DistrictQueries.AddDistrict, requestParams, CommandType.StoredProcedure);

                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";

                logging.LogInfo($"District {request.DistrictName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for District: {request.DistrictName}", ex);
                throw;
            }
        }
        public async Task UpdateDistrictAsync(DistrictUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "DistrictId", request.DistrictId},
                    { "DistrictName", request.DistrictName ?? (object)DBNull.Value },
                    { "StateId", request.StateId ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(DistrictQueries.AddDistrict, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"District {request.DistrictName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for District: {request.DistrictName}", ex);
                throw;
            }
        }
        public async Task<DistrictResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for user id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<DistrictResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<DistrictResponse>(DistrictQueries.GetDistrict, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "DistrictId", id } 
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new DistrictResponse();
                logging.LogInfo(result != null
                    ? $"District with id {id} retrieved successfully."
                    : $"District with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for District id: {id}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<DistrictResponse>> GetDistricts(DistrictRequest request) 
        { 
            var repository = repositoryFactory.Connect<DistrictResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<DistrictResponse>(DistrictQueries.GetDistrict, requestParams, null, CommandType.StoredProcedure);

            return response;
        }
        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"DistrictId", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };

                var response = await repository.DeleteAsync(
                   DistrictQueries.AddDistrict, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"District with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for District id: {id}", ex);
                throw;
            }

        }
        public async Task<IListsResponse<DistrictResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<DistrictResponse, int, FiltersMeta>(DistrictQueries.GetDistrictList,
                    DbConstants.Main, parameters,
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
            return new ListsResponse<DistrictResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

    }
}
