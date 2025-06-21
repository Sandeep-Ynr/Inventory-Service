using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Response;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IDistrictService
    {
        Task<IEnumerable<DistrictResponse>> GetDistricts(DistrictRequest request);

        Task<IEnumerable<CommonLists>> GetSpecificLists(DistrictRequest request);
        
        Task<string> AddDistrictsAsync(DistrictRequest request);
    }
}
