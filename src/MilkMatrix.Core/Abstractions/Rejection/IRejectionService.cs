using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Request.Rejection;
using MilkMatrix.Core.Entities.Response.Rejection;

namespace MilkMatrix.Core.Abstractions.Rejection;

public interface IRejectionService
{
    /// <summary>
    /// Adds a new rejection details to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAsync(IEnumerable<InsertRejection> request);

    /// <summary>
    /// Retrieves a list of rejection details from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<Details>> GetAllAsync(IListsRequest request);
}
