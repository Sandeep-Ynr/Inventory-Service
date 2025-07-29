using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request.Approval.Level;
using MilkMatrix.Core.Entities.Response.Approval.Details;
using MilkMatrix.Core.Entities.Response.Approval.Level;
using InsertDetails = MilkMatrix.Core.Entities.Request.Approval.Details.Insert;
using InsertLevel = MilkMatrix.Core.Entities.Request.Approval.Level.Insert;

namespace MilkMatrix.Core.Abstractions.Approval.Service;

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
    Task DeleteAsync(int pageId, int businessId);

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

    /// <summary>
    /// Retrieves the approval details for a specific page, business, and record ID.
    /// </summary>
    /// <param name="pageId"></param>
    /// <param name="businessId"></param>
    /// <param name="recordId"></param>
    /// <returns></returns>
    Task<IEnumerable<ApprovalResponse>?> GetPageApprovalDetailsAsync(int pageId, int businessId, string recordId);

    /// <summary>
    /// Approves a collection of requests based on the provided handler key and a function to convert each request to a parameter dictionary.
    /// </summary>
    /// <param name="requests"></param>
    /// <param name="handlerKey"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<StatusCode> ApproveAsync(IEnumerable<InsertDetails> requests, FactoryMapping handlerKey, Func<InsertDetails, Dictionary<string, object>> parameters);
}
