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
using MilkMatrix.Milk.Contracts.Route;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Route;
using MilkMatrix.Milk.Models.Response.Route;
using static MilkMatrix.Milk.Models.Queries.BankQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class RouteService : IRouteService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public RouteService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(RouteService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddRoute(RouteInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "Name", request.Name },
                    { "RouteCode", request.RouteCode },
                    { "CompanyCode", request.CompanyCode },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "VehicleID", request.VehicleID },
                    { "VehicleCapacity", request.VehicleCapacity },
                    { "MorningStartTime", request.MorningStartTime },
                    { "MorningEndTime", request.MorningEndTime },
                    { "EveningStartTime", request.EveningStartTime },
                    { "EveningEndTime", request.EveningEndTime },
                    { "TotalKm", request.TotalKm },
                    { "IsStatus", request.IsActive },
                    { "CreatedBy", request.CreatedBy }
                };

                var response = await repository.AddAsync(RouteQueries.AddRoute, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Route {request.Name} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Route: {request.Name}", ex);
                throw;
            }
        }

        public async Task UpdateRoute(RouteUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RouteID", request.RouteID },
                    { "Name", request.Name },
                    { "RouteCode", request.RouteCode },
                    { "CompanyCode", request.CompanyCode },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "VehicleID", request.VehicleID },
                    { "VehicleCapacity", request.VehicleCapacity },
                    { "MorningStartTime", request.MorningStartTime },
                    { "MorningEndTime", request.MorningEndTime },
                    { "EveningStartTime", request.EveningStartTime },
                    { "EveningEndTime", request.EveningEndTime },
                    { "TotalKm", request.TotalKm },
                    { "IsStatus", request.IsActive },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };

                await repository.UpdateAsync(RouteQueries.AddRoute, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Route {request.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Route Name: {request.Name}", ex);
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
                    { "RouteID", id },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(RouteQueries.AddRoute, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Route with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Route ID: {id}", ex);
                throw;
            }
        }

        public async Task<RouteResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Route ID: {id}");
                var repo = repositoryFactory.ConnectDapper<RouteResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<RouteResponse>(RouteQueries.GetRouteList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RouteID", id }
                }, null);

                var result = data.FirstOrDefault() ?? new RouteResponse();
                logging.LogInfo(result != null
                    ? $"Route with ID {id} retrieved successfully."
                    : $"Route with ID {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Route ID: {id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<RouteResponse>> GetRoutes(RouteRequest request)
        {
            var repository = repositoryFactory.Connect<RouteResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "RouteID", (int)request.RouteID },
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsActive }
            };
            return await repository.QueryAsync<RouteResponse>(RouteQueries.GetRouteList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(RouteRequest request)
        {
            var repository = repositoryFactory.Connect<RouteResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsActive }
            };
            return await repository.QueryAsync<CommonLists>(RouteQueries.GetRouteList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IListsResponse<RouteResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<RouteResponse, int, FiltersMeta>(RouteQueries.GetRouteList,
                    DbConstants.Main,
                    parameters,
                    null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<RouteResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
