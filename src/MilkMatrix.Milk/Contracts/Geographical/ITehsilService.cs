using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical;

public interface ITehsilService
{
    Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request);
    Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request);
    Task<string> AddTehsil(TehsilRequest request);
}
