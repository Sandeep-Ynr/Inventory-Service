using MilkMatrix.Api.Models.Request.PriceApplicability;
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

        Task<IListsResponse<RateForInsertResponse>> GetAllRateForAsync(IListsRequest request);
        Task<PriceActualRateResponse?> GetRateForByIdAsync(PriceAppRateforRequest request);
        Task AddRateForAsync(RateForInsertRequest request);
        Task UpdateRateForAsync(RateForUpdateRequest request);
        Task DeleteRateForAsync(int id, int userId);
    }
}
