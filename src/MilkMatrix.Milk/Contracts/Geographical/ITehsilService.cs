using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical;

public interface ITehsilService
{
    Task<TehsilResponse?> GetByIdAsync(int id);
    Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request);
    Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request);
    Task AddTehsilAsync(TehsilInsertRequest request);
    Task UpdateTehsilAsync(TehsilUpdateRequest request);
    Task DeleteAsync(int id, int userId);
    Task<IListsResponse<TehsilResponse>> GetAllAsync(IListsRequest request);
}
