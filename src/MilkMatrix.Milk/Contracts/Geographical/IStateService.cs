using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Response;

namespace MilkMatrix.Milk.Contracts.Geographical;

public interface IStateService
{
    Task<IEnumerable<StateResponse>> GetStates(StateRequest request);
    Task<string> AddStateAsync(StateRequest request);

    Task<IEnumerable<CommonLists>> GetSpecificLists(StateRequest request);
}
