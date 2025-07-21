using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Milk;
using MilkMatrix.Milk.Models.Response.Milk;

namespace MilkMatrix.Milk.Contracts.Milk
{
    public interface IMilkService
    {
        Task<IListsResponse<MilkTypeInsertResponse>> GetAllAsync(IListsRequest request);
        Task<MilkTypeInsertResponse?> GetByIdAsync(int id);
        Task AddAsync(MilkTypeInsertRequest request);
        Task UpdateAsync(MilkTypeUpdateRequest request);
        Task DeleteAsync(int id, int userId);

        Task<IListsResponse<RateTypeInsertResponse>> GetAllRateTypeAsync(IListsRequest request);
        Task<RateTypeInsertResponse?> GetRateTypeByIdAsync(int id);
        Task AddRateTypeAsync(RateTypeInsertRequest request);
        Task UpdateRateTypeAsync(RateTypeUpdateRequest request);
        Task DeleteRateTypeAsync(int id, int userId);
    }
}
