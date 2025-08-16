using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Route.RouteTiming;
using MilkMatrix.Milk.Models.Response.Route.RouteTiming;

namespace MilkMatrix.Milk.Contracts.Route.RouteTiming
{
    public interface IRouteTimingService
    {
        Task InsertRouteTiming(RouteTimingInsertRequest request);
        Task UpdateRouteTiming(RouteTimingUpdateRequest request);
        Task DeleteRouteTiming(int routeTimingId, int userId);
        Task<IEnumerable<CommonLists>> GetSpecificLists(RouteTimingRequest request);
        Task<IListsResponse<RouteTimingResponse>> GetAll(IListsRequest request);
        Task<RouteTimingResponse?> GetById(int routeTimingId);
        Task<IEnumerable<RouteTimingResponse>> GetRouteTimings(RouteTimingRequest request);
    }
}
