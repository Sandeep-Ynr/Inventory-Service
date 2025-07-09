using MilkMatrix.Admin.Models.Admin.Requests.Rejection;
using MilkMatrix.Admin.Models.Admin.Responses.Rejection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IRejectionService
{
    /// <summary>
    /// Adds a new rejection details to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAsync(InsertRejection request);

    /// <summary>
    /// Retrieves a list of rejection details from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<Details>> GetAllAsync(IListsRequest request);
}
