using MilkMatrix.Admin.Models.Admin.Requests.Approval.Level;
using MilkMatrix.Admin.Models.Admin.Responses.Approval.Details;
using MilkMatrix.Admin.Models.Admin.Responses.Approval.Level;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using InsertDetails = MilkMatrix.Admin.Models.Admin.Requests.Approval.Details.Insert;
using InsertLevel = MilkMatrix.Admin.Models.Admin.Requests.Approval.Level.Insert;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for approval service operations in the application.
/// </summary>
public interface IApprovalService
{
    /// <summary>
    /// Retrieves the details of an approval level by its unique identifier.
    /// </summary>
    /// <param name="pageId"></param>
    /// <returns></returns>
    Task<IEnumerable<ApprovalResponse>?> GetByIdAsync(int pageId, int businessId);

    /// <summary>
    /// Adds a new approval level to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAsync(IEnumerable<InsertLevel> requests);

    /// <summary>
    /// Updates an existing approval level in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateAsync(Update request);

    /// <summary>
    /// Deletes an approval level from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="pageId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteAsync(int pageId, int userId);

    /// <summary>
    /// Retrieves a list of approval levels from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<ApprovalResponse>> GetAllAsync(IListsRequest request);

    /// <summary>
    /// Adds details to an approval details in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddDetailsAsync(IEnumerable<InsertDetails> request);

    /// <summary>
    /// Retrieves a list of approval details from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<ApprovalDetails>> GetAllDetailsAsync(IListsRequest request);
}
