using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;


namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IHamletService
    {
        Task<IEnumerable<HamletResponse>> GetHamlets(HamletRequest request);
        Task<IEnumerable<HamletRequest>> GetByHamletId(int hamletId);

        Task<IEnumerable<CommonLists>> GetSpecificLists(HamletRequest request);
        Task<string> AddHamlet(HamletInsertRequest request);
        Task<string> UpdateHamlet(HamletUpdateRequest request);
        Task<string> DeleteAsync(int id, int userId);

        
    }
}
