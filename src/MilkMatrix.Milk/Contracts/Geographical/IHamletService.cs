using MilkMatrix.Domain.Entities.Common;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;


namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IHamletService
    {
        Task<IEnumerable<HamletResponse>> GetHamlets(HamletRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(HamletRequest request);

        Task<string> AddHamlet(HamletRequest request);
    }
}
