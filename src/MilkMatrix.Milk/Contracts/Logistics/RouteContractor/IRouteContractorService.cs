using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Request.Route.RouteContractor;
using MilkMatrix.Milk.Models.Response.MPP;
using MilkMatrix.Milk.Models.Response.Route.RouteContractor;

namespace MilkMatrix.Milk.Contracts.Route.RouteContractor
{
    public interface IRouteContractorService
    {
        Task InsertRouteContractor(RouteContractorInsertRequest request);
        Task<RouteContractorResponse> GetRouteContractorById(int routeContractorId);
        Task<IEnumerable<RouteContractorResponse>> GetAllRouteContractors();
        Task UpdateRouteContractor(RouteContractorUpdateRequest request);
        Task DeleteRouteContractor(int routeContractorId, int userId);
        Task<IListsResponse<RouteContractorResponse>> GetAll(ListsRequest request);

       
    }
}

