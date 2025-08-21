using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Milk.Models.Response.ConfigSettings;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.ConfigSettings;

namespace MilkMatrix.Milk.Contracts.ConfigSettings
{
    public interface IConfigSettingService
    {
        Task InsertConfigSetting(ConfigSettingInsertRequest request);
        Task UpdateConfigSetting(ConfigSettingUpdateRequest request);
        Task DeleteConfigSetting(int BusinessId, string UnitType, string UnitIds);
        Task<IEnumerable<CommonLists>> GetSpecificLists(ConfigSettingRequest request);
        Task<IListsResponse<ConfigSettingResponse>> GetAll(IListsRequest request);
        Task<ConfigSettingResponse?> GetById(int configId);
        Task<IEnumerable<ConfigSettingResponse>> GetConfigSettings(ConfigSettingRequest request);
    }
}
