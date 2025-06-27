using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IVillageService
    {

        Task<string> AddVillage(VillageRequest request);
        Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request);

        Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request);

    }
}
