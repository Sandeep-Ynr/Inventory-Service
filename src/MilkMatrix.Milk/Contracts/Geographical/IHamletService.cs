using MilkMatrix.Domain.Entities.Common;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Response;


namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IHamletService
    {
        Task<IEnumerable<HamletResponse>> GetHamlets(HamletRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(HamletRequest request);
    }
}
