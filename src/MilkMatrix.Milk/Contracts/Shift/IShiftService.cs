using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Shift;
using MilkMatrix.Milk.Models.Response.Shift;

namespace MilkMatrix.Milk.Contracts.Shift
{
    public interface IShiftService
    {
        Task<IListsResponse<ShiftInsertResponse>> GetAllAsync(IListsRequest request);
        Task<ShiftInsertResponse?> GetByIdAsync(int id);
        Task AddAsync(ShiftInsertRequest request);
        Task UpdateAsync(ShiftUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
