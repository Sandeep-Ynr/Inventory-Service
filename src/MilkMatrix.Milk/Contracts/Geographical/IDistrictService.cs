using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IDistrictService
    {
        Task<DistrictResponse?> GetByIdAsync(int id);
        Task<IEnumerable<DistrictResponse>> GetDistricts(DistrictRequest request);

        Task<IEnumerable<CommonLists>> GetSpecificLists(DistrictRequest request);
        
        Task<string> AddDistrictsAsync(DistrictRequest request);
        Task<string> UpdateDistrictAsync(DistrictUpdateRequest request);
        Task<string> DeleteAsync(int id, int userId);

    }
}
