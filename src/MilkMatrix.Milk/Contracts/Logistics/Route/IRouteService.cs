using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Logistics.Route;
using MilkMatrix.Milk.Models.Response.Logistics.Route;

namespace MilkMatrix.Milk.Contracts.Logistics.Route
{
    public interface IRouteService
    {
        Task AddRoute(RouteInsertRequest request);
        Task UpdateRoute(RouteUpdateRequest request);
        Task Delete(int id, int userId);
        Task<IEnumerable<CommonLists>> GetSpecificLists(RouteRequest request);
        Task<IListsResponse<RouteResponse>> GetAll(IListsRequest request);
        Task<RouteResponse?> GetById(int id);
        Task<IEnumerable<RouteResponse>> GetRoutes(RouteRequest request);
    }
}

