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
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of user login details.</returns>
    [HttpPost("login-details")]
    public async Task<IActionResult> GetLoginDetails([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetLoginDetailsAsync(request);
        return Ok(response);
    }
    /// <summary>
    /// Gets the event logs.
    /// </summary>
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of event logs.</returns>
    [HttpPost("event-logs")]
    public async Task<IActionResult> GetEventLogs([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetEventLogsAsync(request);
        return Ok(response);
    }
    /// <summary>
    /// Gets the audit logs.
    /// </summary>
    /// <param name="request">The request containing pagination and filter information.</param>
    /// <returns>A list of audit logs.</returns>
    [HttpPost("audit-logs")]
    public async Task<IActionResult> GetAuditLogs([FromBody] ListsRequest request)
    {
        var response = await _reportService.GetAuditLogsAsync(request);
        return Ok(response);
    }
}
