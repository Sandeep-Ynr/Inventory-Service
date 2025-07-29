using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Core.Entities.Request;

namespace MilkMatrix.Api.Controllers.v1;

/// <summary>
/// Controller for handling report-related operations.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    /// <summary>
    /// Initializes a new instance of the <see cref="ReportController"/> class.
    /// </summary>
    /// <param name="reportService">The report service to use for report operations.</param>
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Gets the login details of users.
    /// </summary>
    /// <remarks>
    /// Retrieves a paginated, filterable list of user login sessions.
    /// 
    /// <b>Request Body (ListsRequest):</b>
    /// {
    ///   "limit": 50,
    ///   "offset": 0,
    ///   "search": "optional search string",
    ///   "filters": {
    ///     "UserId": 123,
    ///     "LoginDate": "2025-07-24 00:00:00.000 - 2025-07-25 23:59:59.999",
    ///     "LogOutDate": "2025-07-24 00:00:00.000 - 2025-07-25 23:59:59.999"
    ///   },
    ///   "sort": {
    ///     "LoginDate": "DESC"
    ///   }
    /// }
    /// 
    /// <b>Response (IListsResponse&lt;LoginDetails&gt;):</b>
    /// {
    ///   "count": 100,
    ///   "results": [
    ///     {
    ///       "id": 1,
    ///       "userId": 123,
    ///       "userName": "john.doe",
    ///       "loginDate": "2025-07-24T08:00:00",
    ///       "logOutDate": "2025-07-24T17:00:00"
    ///     }
    ///   ],
    ///   "filters": [ /* Filter meta information */ ]
    /// }
    /// 
    /// <b>Notes:</b>
    /// - Filters are optional and case-insensitive.
    /// - Date ranges must be in the format: "startDateTime - endDateTime".
    /// </remarks>
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of user login details.</returns>
    /// <response code="200">Returns the paginated login details.</response>
    [HttpPost("login-details")]
    public async Task<IActionResult> GetLoginDetails([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetLoginDetailsAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Gets the event logs.
    /// </summary>
    /// <remarks>
    /// Retrieves a paginated, filterable list of event logs.
    /// 
    /// <b>Request Body (ListsRequest):</b>
    /// {
    ///   "limit": 50,
    ///   "offset": 0,
    ///   "search": "optional search string",
    ///   "filters": {
    ///     "UserId": 123,
    ///     "ActionDate": "2025-07-24 00:00:00.000 - 2025-07-25 23:59:59.999"
    ///   },
    ///   "sort": {
    ///     "ActionDate": "DESC"
    ///   }
    /// }
    /// 
    /// <b>Response (IListsResponse&lt;EventLogs&gt;):</b>
    /// {
    ///   "count": 100,
    ///   "results": [
    ///     {
    ///       "id": 1,
    ///       "userId": 123,
    ///       "userName": "john.doe",
    ///       "pageId": 2,
    ///       "pageName": "Dashboard",
    ///       "actionId": "A1",
    ///       "actionName": "Login",
    ///       "actionDocNo": "DOC123",
    ///       "actionDate": "2025-07-24T09:00:00",
    ///       "businessId": 10,
    ///       "businessName": "Acme Corp"
    ///     }
    ///   ],
    ///   "filters": [ /* Filter meta information */ ]
    /// }
    /// 
    /// <b>Notes:</b>
    /// - Filters are optional and case-insensitive.
    /// - Date ranges must be in the format: "startDateTime - endDateTime".
    /// </remarks>
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of event logs.</returns>
    /// <response code="200">Returns the paginated event logs.</response>
    [HttpPost("event-logs")]
    public async Task<IActionResult> GetEventLogs([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetEventLogsAsync(request);
        return Ok(response);
    }
    /// <summary>
    /// Gets the audit logs.
    /// </summary>
    /// <remarks>
    /// Retrieves a paginated, filterable list of audit logs.
    /// 
    /// <b>Request Body (ListsRequest):</b>
    /// {
    ///   "limit": 50,
    ///   "offset": 0,
    ///   "search": "optional search string",
    ///   "filters": {
    ///     "UserId": 123,
    ///     "UpdatedOn": "2025-07-24 00:00:00.000 - 2025-07-25 23:59:59.999"
    ///   },
    ///   "sort": {
    ///     "UpdatedOn": "DESC"
    ///   }
    /// }
    /// 
    /// <b>Response (IListsResponse&lt;AuditLogs&gt;):</b>
    /// {
    ///   "count": 100,
    ///   "results": [
    ///     {
    ///       "id": 1,
    ///       "userId": 123,
    ///       "userName": "john.doe",
    ///       "pageId": 2,
    ///       "pageName": "Settings",
    ///       "columnName": "Status",
    ///       "oldValue": "Inactive",
    ///       "newValue": "Active",
    ///       "updatedOn": "2025-07-24T10:00:00",
    ///       "recordNo": "REC001",
    ///       "tableName": "Users"
    ///     }
    ///   ],
    ///   "filters": [ /* Filter meta information */ ]
    /// }
    /// 
    /// <b>Notes:</b>
    /// - Filters are optional and case-insensitive.
    /// - Date ranges must be in the format: "startDateTime - endDateTime".
    /// </remarks>
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of audit logs.</returns>
    /// <response code="200">Returns the paginated audit logs.</response>

    [HttpPost("audit-logs")]
    public async Task<IActionResult> GetAuditLogs([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetAuditLogsAsync(request);
        return Ok(response);
    }
}
