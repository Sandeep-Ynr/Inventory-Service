using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Response.Logistics.Vehicle;


namespace MilkMatrix.Milk.Contracts.Logistics.Vehicle
{
    public interface IVehicleService
    {
        Task AddVehicle(VehicleInsertRequest request);
        Task UpdateVehicle(VehicleUpdateRequest request);
        Task Delete(int vehicleId, int userId);
        Task<VehicleResponse?> GetById(int id);
        Task<IEnumerable<VehicleResponse>> GetVehicles(VehicleRequest request);
        Task<IListsResponse<VehicleResponse>> GetAll(IListsRequest request);
    }
}
