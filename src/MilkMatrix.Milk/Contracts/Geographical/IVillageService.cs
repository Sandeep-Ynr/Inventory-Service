using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Response;

namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IVillageService
    {
        Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request);

        Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request);

    }
}
