using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical;

public interface ITehsilService
{
    Task<TehsilResponse?> GetByIdAsync(int id);
    Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request);
    Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request);
    Task<string> AddTehsilAsync(TehsilInsertRequest request);
    //Task<string> AddTehsil(TehsilRequest request);
    Task<string> UpdateTehsilAsync(TehsilUpdateRequest request);
    Task<string> DeleteAsync(int id, int userId);
}
