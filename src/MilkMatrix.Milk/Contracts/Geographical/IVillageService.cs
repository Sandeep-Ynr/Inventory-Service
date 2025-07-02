using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IVillageService
    {

        Task<string> AddVillage(VillageRequest request);
        Task<string> UpdateVillage(VillageRequest request);
        Task<string> DeleteVillage(int id);
        Task<IEnumerable<VillageRequest>> GetByVillageId(int villageId);
        Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request);

    }
}
