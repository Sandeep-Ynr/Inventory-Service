using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Price;
using MilkMatrix.Milk.Models.Response.Price;

namespace MilkMatrix.Milk.Contracts.Price
{
    public interface IPriceService
    {
        Task<IListsResponse<MilkPriceInsertResponse>> GetAllAsync(IListsRequest request);
        Task<MilkPriceInsertResponse?> GetByIdAsync(int id);
        Task AddAsync(MilkPriceInsertRequest request);
        Task UpdateAsync(MilkPriceUpdateRequest request);
        Task DeleteAsync(int id, int userId);

        Task AddBulkUsersAsync(byte[] bytes, int userId);
    }
}
