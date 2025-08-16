using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Route.RouteContractor;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Route.RouteContractor;
using MilkMatrix.Milk.Models.Response.Route.RouteContractor;

namespace MilkMatrix.Milk.Implementations.Route.RouteContractor
{
    public class RouteContractorService : IRouteContractorService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public RouteContractorService(
            ILogging logging,
            IOptions<AppConfig> appConfig,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(RouteContractorService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task DeleteRouteContractor(int routeContractorId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
        {
            { "ActionType", (int)CrudActionType.Delete },
            { "RouteContractorId", routeContractorId }
        };

                var response = await repository.DeleteAsync(
                    RouteContractorQueries.AddRouteContractor,
                    requestParams,
                    CommandType.StoredProcedure
                );

                logging.LogInfo($"Route Contractor with id {routeContractorId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Route Contractor id: {routeContractorId}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<RouteContractorResponse>> GetAll(ListsRequest request)
        {
            var parameters = new Dictionary<string, object>
    {
        { "ActionType", (int)ReadActionType.All }

    };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<RouteContractorResponse, int, FiltersMeta>(
                    RouteContractorQueries.GetRouteContractorList,
                    DbConstants.Main,
                    parameters,
                    null
                );

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
            return new ListsResponse<RouteContractorResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }


        public async Task<RouteContractorResponse?> GetRouteContractorById(int routeContractorId)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Route Contractor id: {routeContractorId}");
                var repo = repositoryFactory
                           .ConnectDapper<RouteContractorResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<RouteContractorResponse>(
                    RouteContractorQueries.GetRouteContractorList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "RouteContractorId", routeContractorId }
                            },
                            null
                        );

                var result = data.Any() ? data.FirstOrDefault() : new RouteContractorResponse();
                logging.LogInfo(result != null
                    ? $"Route Contractor with id {routeContractorId} retrieved successfully."
                    : $"Route Contractor with id {routeContractorId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Route Contractor id: {routeContractorId}", ex);
                throw;
            }
        }


        public async Task InsertRouteContractor(RouteContractorInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create },          
                        { "business_Id", request.BusinessId },
                        { "ContractorName", request.ContractorName },
                        { "ContactNumber", request.ContactNumber ?? (object)DBNull.Value },
                        { "ContractorAddress", request.Address ?? (object)DBNull.Value },
                        { "IsStatus", request.IsStatus  },
                        { "CreatedBy", request.CreatedBy ?? 0 }
                    };

                var message = await repository.AddAsync(RouteContractorQueries.AddRouteContractor, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Route Contractor {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in InsertRouteContractor: {request.ContractorName}", ex);
                throw;
            }
        }

        public async Task UpdateRouteContractor(RouteContractorUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Update },
                        { "RouteContractorId", request.RouteContractorId },
                        { "ContractorName", request.ContractorName },
                        { "ContactNumber", request.ContactNumber },
                        { "ContractorAddress", request.Address },
                        { "IsStatus", request.IsStatus  },
                        { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value }
                    };

                var message = await repository.UpdateAsync(RouteContractorQueries.AddRouteContractor, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Contractor {request.ContractorName} updated successfully.");
                }
            }
            catch (Exception ex) 
            {
                logging.LogError($"Error in UpdateContractor: {request.ContractorName}", ex);
                throw;
            }
        }


        public Task<IEnumerable<RouteContractorResponse>> GetAllRouteContractors()
        {
            throw new NotImplementedException();
        }


        //public Task<RouteContractorResponse> GetRouteContractorById(int routeContractorId)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<RouteContractorResponse> GetRouteContractorById(int routeContractorId)
        //{
        //    try
        //    {
        //        logging.LogInfo($"GetRouteContractorById called for ID: {routeContractorId}");

        //        var repo = repositoryFactory.ConnectDapper<RouteContractorResponse>(DbConstants.Main);

        //        var data = await repo.QueryAsync<RouteContractorResponse>(
        //            RouteContractorQueries.GetRouteContractorList,
        //            new Dictionary<string, object>
        //            {
        //                { "ActionType", (int)ReadActionType.Individual },
        //                { "RouteContractorId", routeContractorId }
        //            },
        //            null
        //        );

        //        return data.FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in GetRouteContractorById for ID: {routeContractorId}", ex);
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<RouteContractorResponse>> GetAllRouteContractors()
        //{
        //    try
        //    {
        //        var repo = repositoryFactory.ConnectDapper<RouteContractorResponse>(DbConstants.Main);

        //        var data = await repo.QueryAsync<RouteContractorResponse>(
        //            RouteContractorQueries.GetRouteContractorList,
        //            new Dictionary<string, object>
        //            {
        //                { "ActionType", (int)ReadActionType.All }
        //            },
        //            null
        //        );

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError("Error in GetAllRouteContractors", ex);
        //        throw;
        //    }
        //}

        //public async Task<RouteContractorResponse> UpdateRouteContractor(RouteContractorUpdateRequest request)
        //{
        //    try
        //    {
        //        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

        //        var requestParams = new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)CrudActionType.Update },
        //            { "RouteContractorId", request.RouteContractorId },
        //            { "ContractorName", request.ContractorName },
        //            { "ContactNumber", request.ContactNumber },
        //            { "Address", request.Address },
        //            { "IsStatus", request.IsStatus  },
        //            { "ModifiedOn", request.ModifiedOn ?? (object)DBNull.Value },
        //            { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value }
        //        };

        //        var result = await repository.UpdateAsync(RouteContractorQueries.AddRouteContractor, requestParams, CommandType.StoredProcedure);

        //        if (result.StartsWith("Error"))
        //            throw new Exception($"Stored Procedure Error: {result}");

        //        return await GetRouteContractorById(request.RouteContractorId);
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in UpdateRouteContractor: {request.RouteContractorId}", ex);
        //        throw;
        //    }
        //}

        //public async Task<bool> DeleteRouteContractor(int routeContractorId, int userId)
        //{
        //    try
        //    {
        //        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

        //        var requestParams = new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)CrudActionType.Delete },
        //            { "RouteContractorId", routeContractorId },
        //            { "ModifiedBy", userId }
        //        };

        //        var result = await repository.DeleteAsync(RouteContractorQueries.AddRouteContractor, requestParams, CommandType.StoredProcedure);

        //        if (result.StartsWith("Error"))
        //            throw new Exception($"Stored Procedure Error: {result}");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in DeleteRouteContractor for ID: {routeContractorId}", ex);
        //        throw;
        //    }
        //}

        //public async Task<IListsResponse<RouteContractorResponse>> GetAll(ListsRequest request)
        //{
        //    try
        //    {
        //        var repo = repositoryFactory.ConnectDapper<RouteContractorResponse>(DbConstants.Main);

        //        var parameters = new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)ReadActionType.All },
        //            { "Search", request.Search },
        //            { "Limit", request.Limit },
        //            { "Offset", request.Offset }
        //        };

        //        if (request.Filters != null)
        //        {
        //            foreach (var filter in request.Filters)
        //            {
        //                parameters.Add(filter.Key, filter.Value ?? DBNull.Value);
        //            }
        //        }

        //        var result = await repo.QueryAsync<RouteContractorResponse>(RouteContractorQueries.GetRouteContractorList, parameters, null);

        //        return new ListsResponse<RouteContractorResponse>
        //        {
        //            Results = result,
        //            Count = result.Count()
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError("Error in GetAll RouteContractors (paginated)", ex);
        //        throw;
        //    }
        //}


    }
}
