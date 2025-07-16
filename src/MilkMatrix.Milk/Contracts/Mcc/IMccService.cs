using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Mcc;
using MilkMatrix.Milk.Models.Response.Mcc;

namespace MilkMatrix.Milk.Contracts.Mcc
{
    public interface IMccService
    {
        Task<IListsResponse<MccResponse>> GetAllAsync(IListsRequest request);
        Task<MccIndividualResponse?> GetByIdAsync(int id);
        Task AddAsync(MccInsertRequest request);
        Task UpdateAsync(MccUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
