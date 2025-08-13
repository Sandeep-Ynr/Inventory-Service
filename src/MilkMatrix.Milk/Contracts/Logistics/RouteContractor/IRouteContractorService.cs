using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Milk.Models.Request.Route.RouteContractor;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Response.Route.RouteContractor;

namespace MilkMatrix.Milk.Contracts.Route.RouteContractor
{
    public interface IRouteContractorService
    {
        Task<RouteContractorResponse> InsertRouteContractor(RouteContractorInsertRequest request);
        Task<RouteContractorResponse> GetRouteContractorById(int routeContractorId);
        Task<IEnumerable<RouteContractorResponse>> GetAllRouteContractors();
        Task<RouteContractorResponse> UpdateRouteContractor(RouteContractorUpdateRequest request);
        Task<bool> DeleteRouteContractor(int routeContractorId, int userId);
        Task<IListsResponse<RouteContractorResponse>> GetAll(ListsRequest request);
    }
}

