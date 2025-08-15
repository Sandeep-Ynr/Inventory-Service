using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
using MilkMatrix.Milk.Contracts.Route.RouteTiming;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Route.RouteTiming;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Response.Route.RouteTiming;
using static MilkMatrix.Milk.Models.Queries.BankQueries;

namespace MilkMatrix.Milk.Implementations.Route.RouteTiming
{
    public class RouteTimingService : IRouteTimingService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public RouteTimingService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(RouteTimingService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task InsertRouteTiming(RouteTimingInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "RouteId", request.RouteId },
                    { "EffectiveDate", request.EffectiveDate },
                    { "MorningTime", request.MorningTime },
                    { "EveningTime", request.EveningTime },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value }
                };
                var message = await repository.AddAsync(RouteTimeQueries.AddUpdateRouteTiming, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"RouteTiming for RouteId {request.RouteId} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error adding RouteTiming for RouteId: {request.RouteId}", ex);
                throw;
            }
        }

        public async Task UpdateRouteTiming(RouteTimingUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RouteScheduleId", request.RouteTimingId },
                    { "RouteId", request.RouteId },
                    { "EffectiveDate", request.EffectiveDate },
                    { "MorningTime", request.MorningTime },
                    { "EveningTime", request.EveningTime },
                    { "IsStatus", request.IsStatus?? (object)DBNull.Value  },
                    { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value }
                };
                var message = await repository.UpdateAsync(RouteTimeQueries.AddUpdateRouteTiming, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"RouteTiming for RouteId {request.RouteId} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error adding RouteTiming for RouteId: {request.RouteId}", ex);
                throw;
            }
        }

        public async Task DeleteRouteTiming(int routeTimingId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "RouteScheduleId", routeTimingId },
                    { "ModifiedBy", userId }
                };
                var message = await repository.UpdateAsync(RouteTimeQueries.AddUpdateRouteTiming, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"RouteTiming deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error RouteTiming ", ex);
                throw;
            }

        }

        public async Task<RouteTimingResponse?> GetById(int routeTimingId)
        {
            try
            {
                logging.LogInfo($"GetById called for RouteTiming ID: {routeTimingId}");
                var repo = repositoryFactory.ConnectDapper<RouteTimingResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<RouteTimingResponse>(RouteTimeQueries.GetRouteTimingList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RouteScheduleId", routeTimingId }
                }, null);

                var result = data.FirstOrDefault();
                logging.LogInfo(result != null
                    ? $"RouteTiming with ID {routeTimingId} retrieved successfully."
                    : $"RouteTiming with ID {routeTimingId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error retrieving RouteTiming with ID: {routeTimingId}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<RouteTimingResponse>> GetRouteTimings(RouteTimingRequest request)
        {
            var repository = repositoryFactory.Connect<RouteTimingResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "RouteTimingId", request.RouteTimingId },
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<RouteTimingResponse>(RouteTimeQueries.AddUpdateRouteTiming, requestParams, null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<CommonLists>> GetSpecificLists(RouteTimingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IListsResponse<RouteTimingResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<RouteTimingResponse, int, FiltersMeta>(RouteTimeQueries.GetRouteTimingList,
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
            return new ListsResponse<RouteTimingResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
