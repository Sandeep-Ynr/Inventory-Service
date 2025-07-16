using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Bmc;
using MilkMatrix.Milk.Models.Response.Bmc;

namespace MilkMatrix.Milk.Contracts.Bmc
{
    public interface IBmcService
    {
        Task<IListsResponse<BmcResponse>> GetAllAsync(IListsRequest request);
        Task<BmcIndividualResponse?> GetByIdAsync(int id);
        Task AddAsync(BmcInsertRequest request);
        Task UpdateAsync(BmcUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
