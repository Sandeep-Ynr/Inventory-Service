using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.PriceApplicability;
using MilkMatrix.Milk.Models.Response.PriceApplicability;

namespace MilkMatrix.Milk.Contracts.PriceApplicability
{
    public interface IPriceApplicabilityService
    {
        Task<IListsResponse<PriceAppInsertResponse>> GetAllAsync(IListsRequest request);
        Task<PriceAppInsertResponse?> GetByIdAsync(int id);
        Task AddAsync(PriceAppInsertRequest request);
        Task UpdateAsync(PriceAppUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
