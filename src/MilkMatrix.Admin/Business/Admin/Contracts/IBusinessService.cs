using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response.Business;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for business service operations in the application.
/// </summary>
public interface IBusinessService
{
    /// <summary>
    /// Retrieves the details of a business by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BusinessDetails?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new business to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAsync(BusinessInsertRequest request);

    /// <summary>
    /// Updates an existing business in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateAsync(BusinessUpdateRequest request);

    /// <summary>
    /// Gets a list of businesses from the system.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<BusinessDetails>> GetAllAsync(IListsRequest request);

    /// <summary>
    /// Retrieves a list of business data associated with a user based on their unique identifier and action type.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="actionType"></param>
    /// <returns></returns>
    Task<IEnumerable<BusinessMaster?>> GetBusinessDataByUserIdAsync(BusinessDataRequest request);
}
