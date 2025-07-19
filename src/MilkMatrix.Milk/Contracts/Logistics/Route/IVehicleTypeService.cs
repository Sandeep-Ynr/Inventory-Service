using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Logistics.VehcleType;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleType;

namespace MilkMatrix.Milk.Contracts.Logistics.Route
{
    public interface IVehicleTypeService
    {
        Task AddVehicleType(VehicleTypeInsertRequest request);
        Task UpdateVehicleType(VehicleTypeUpdateRequest request);
        Task Delete(int vehicleId, int userId);
        Task<IEnumerable<CommonLists>> GetSpecificLists(VehicleTypeRequest request);
        Task<IListsResponse<VehicleTypeResponse>> GetAll(IListsRequest request);
        Task<VehicleTypeResponse?> GetById(int vehicleId);
        Task<IEnumerable<VehicleTypeResponse>> GetVehicleTypes(VehicleTypeRequest request);
    }
}
