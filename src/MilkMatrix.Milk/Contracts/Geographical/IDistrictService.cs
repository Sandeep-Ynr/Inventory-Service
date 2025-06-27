using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IDistrictService
    {
        Task<IEnumerable<DistrictResponse>> GetDistricts(DistrictRequest request);

        Task<IEnumerable<CommonLists>> GetSpecificLists(DistrictRequest request);
        
        Task<string> AddDistrictsAsync(DistrictRequest request);
    }
}
