using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;


namespace MilkMatrix.Milk.Contracts.Geographical
{
    public interface IHamletService
    {
        Task<HamletResponse?> GetByHamletId(int hamletId);
        Task<IEnumerable<HamletResponse>> GetHamlets(HamletRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(HamletRequest request);
        Task AddHamlet(HamletInsertRequest request);
        Task UpdateHamlet(HamletUpdateRequest request);
        Task DeleteAsync(int id, int userId);
        Task<IListsResponse<HamletResponse>> GetAllAsync(IListsRequest request);
    }
}
