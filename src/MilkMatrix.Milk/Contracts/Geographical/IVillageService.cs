using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IVillageService
    {

        Task<string> AddVillage(VillageInsertRequest request);
        Task<string> UpdateVillage(VillageUpdateRequest request);
        Task<string> DeleteAsync(int id, int userId);

        Task<VillageResponse> GetByVillageId(int villageId);
        Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request);

    }
}
