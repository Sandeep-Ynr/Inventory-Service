using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;

namespace MilkMatrix.Milk.Contracts.Geographical;

public interface IStateService
{
    Task<IEnumerable<StateResponse>> GetStates(StateRequest request);
    Task<IEnumerable<CommonLists>> GetSpecificLists(StateRequest request);
    Task<string> AddStateAsync(StateInsertRequest request);
    Task<string> UpdateStateAsync(StateUpdateRequest request);
    Task<string> DeleteAsync(int id, int userId);
}
