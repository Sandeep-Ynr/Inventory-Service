using MilkMatrix.Admin.Models.Admin.Responses.Report;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for report service operations in the application.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Retrieves a list of login details for users in the system.
    /// </summary>
    /// <returns>A collection of login details.</returns>
    Task<IListsResponse<LoginDetails>> GetLoginDetailsAsync(IListsRequest request);
    /// <summary>
    /// Retrieves a list of event logs from the system.
    /// </summary>
    /// <returns>A collection of event logs.</returns>
    Task<IListsResponse<EventLogs>> GetEventLogsAsync(IListsRequest request);
    /// <summary>
    /// Retrieves a list of audit logs from the system.
    /// </summary>
    /// <returns>A collection of audit logs.</returns>
    Task<IListsResponse<AuditLogs>> GetAuditLogsAsync(IListsRequest request);
}
