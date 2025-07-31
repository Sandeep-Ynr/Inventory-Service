using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleBillingType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Logistics.VehicleBillingType
{
    public interface IVehicleBillingTypeService
    {
        Task AddVehicleBillingType(VehicleBillingTypeInsertRequest request);
        Task UpdateVehicleBillingType(VehicleBillingTypeUpdateRequest request);
        Task DeleteVehicleBillingType(long id, string deletedBy);
        Task<VehicleBillingTypeResponse?> GetVehicleBillingTypeById(long id);
        Task<IListsResponse<VehicleBillingTypeResponse>> GetAllVehicleBillingTypes(IListsRequest request);

    }
}
