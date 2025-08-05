
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Milk.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Response.MPP;

namespace MilkMatrix.Milk.Contracts.Milk.DeviceSetting
{
    public interface IDeviceSettingService
    {
        Task InsertDeviceSetting(DeviceSettingInsertRequest request);
        Task UpdateDeviceSetting(DeviceSettingUpdateRequest request);
        Task DeleteDeviceSetting(int deviceSettingId, int userId);
        Task<DeviceSettingResponse?> GetDeviceSettingById(int deviceSettingId);
        Task<IListsResponse<DeviceSettingResponse>> GetAll(ListsRequest request);
    }
}
