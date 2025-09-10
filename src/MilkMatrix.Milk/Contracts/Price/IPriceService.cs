using System.Data;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Price;
using MilkMatrix.Milk.Models.Response.Price;

namespace MilkMatrix.Milk.Contracts.Price
{
    public interface IPriceService
    {
        Task<IListsResponse<MilkPriceInsertResp>> GetAllAsync(IListsRequest request);
        Task<MilkPriceInsertResponse?> GetByIdAsync(string ratecode);
        Task<object> GetMilkFatChartJsonAsync(int rateCode);
        Task AddAsync(MilkPriceInsertRequest request);
        Task UpdateAsync(MilkPriceUpdateRequest request);
        Task DeleteAsync(string ratecode, int userId);

    }
}
