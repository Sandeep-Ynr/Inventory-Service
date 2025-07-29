using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Api.Models.Request.Admin.Rejection;
using MilkMatrix.Core.Abstractions.Approval.Service;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Rejection;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Request.Rejection;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Api.Common.Constants.Constants;
using InsertDetails = MilkMatrix.Core.Entities.Request.Approval.Details.Insert;
using InsertDetailsModel = MilkMatrix.Api.Models.Request.Admin.Approval.Details.InsertModel;
using InsertLevel = MilkMatrix.Core.Entities.Request.Approval.Level.Insert;
using InsertLevelModel = MilkMatrix.Api.Models.Request.Admin.Approval.Level.InsertModel;

namespace MilkMatrix.Api.Controllers.v1;

/// <summary>
/// Controller for managing administrative tasks such as user details, modules, and financial years.
/// This controller is secured and requires authorization for access.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logging;
    private readonly ICommonModules commonModules;
    private readonly IApprovalService approvalService;
    private readonly IRejectionService rejectionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="ihttpContextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="logging"></param>
    /// <param name="commonModules"></param>
    /// <param name="approvalService"></param>
    /// <param name="rejectionService"></param>
    public AdminController(IHttpContextAccessor ihttpContextAccessor,
        IMapper mapper,
        ILogging logging,
        ICommonModules commonModules,
        IApprovalService approvalService, IRejectionService rejectionService)
    {
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.commonModules = commonModules;
        this.logging = logging.ForContext("ServiceName", nameof(AdminController));
        this.approvalService = approvalService;
        this.rejectionService = rejectionService;
    }

    /// <summary>
    /// Retrieves common user details based on the user.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("common-list")]
    public async Task<IActionResult> GetCommonUserDetails()
    {
        var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var mobileId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var response = await commonModules.GetCommonDetails(userId, mobileId);
        return response != null
          ? Ok(response)
          : BadRequest(new ErrorResponse
          {
              StatusCode = (int)HttpStatusCode.BadRequest,
              ErrorMessage = string.Format(ErrorMessage.NotFound)
          });
    }

    /// <summary>
    /// Retrieves a list of modules available to the user.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("module-list")]
    public async Task<IActionResult> GetModules()
    {
        var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var mobileId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var response = await commonModules.GetModulesAsync(userId, mobileId);
        return response != null
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }

    /// <summary>
    /// Retrieves a list of financial years based on the provided request.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("financial-year-list")]
    public async Task<IActionResult> GetFinancialYearList(FinancialYearModel request)
    {
        var result = await commonModules.GetFinancialYearAsync(mapper.Map<FinancialYearRequest>(request));
        return result != null && result.Any() ? Ok(result) : NotFound("No records found");
    }

    /// <summary>
    /// Retrieves a list of actions available.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("action-list")]
    public async Task<IActionResult> GetActions([FromQuery] int? id = null)
    {
        var response = await commonModules.GetActionDetailsAsync(id);
        return response != null && response.Any()
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }

    #region Approval level

    [HttpGet("approval-level/{pageId}/{businessId}")]
    public async Task<ActionResult> GetById(int pageId, int businessId)
    {
        try
        {
            var response = await approvalService.GetByIdAsync(pageId, businessId);
            if (response == null)
            {
                logging.LogInfo($"details with page {pageId} and businessid {businessId} not found.");
                return NoContent();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving details with id: {pageId} and businessId {businessId}", ex);
            return StatusCode(500, "An error occurred while retrieving the details.");
        }
    }

    [HttpPost("approval-level-insert")]
    public async Task<IActionResult> InsertApprovalLevels([FromBody] IEnumerable<InsertLevelModel> request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<IEnumerable<InsertLevel>,IEnumerable<InsertLevelModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await approvalService.AddAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("approval-level-list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await approvalService.GetAllAsync(request);
        return Ok(result);
    }
    #endregion

    #region Approval details
    [HttpPost("approval-details-insert")]
    public async Task<IActionResult> InsertApprovalDetails([FromBody]IEnumerable<InsertDetailsModel> request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<IEnumerable<InsertDetails>,IEnumerable<InsertDetailsModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await approvalService.AddDetailsAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("approval-details-list")]
    public async Task<IActionResult> DetailsList([FromBody] ListsRequest request)
    {
        var result = await approvalService.GetAllDetailsAsync(request);
        return Ok(result);
    }

    [HttpGet("page-approval-details")]
    public async Task<IActionResult> GetPageApprovalDetails([FromQuery] int pageId, int businessId, string recordId)
    {
        try
        {
            var response = await approvalService.GetPageApprovalDetailsAsync(pageId, businessId, recordId);
            if (response == null)
            {
                logging.LogInfo($"No approval details found for page {pageId}, business {businessId}, record {recordId}.");
                return NoContent();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving approval details for page {pageId}, business {businessId}, record {recordId}.", ex);
            return StatusCode(500, "An error occurred while retrieving the approval details.");
        }
    }
    #endregion

    #region Rejection

    [HttpPost("rejection-insert")]
    public async Task<IActionResult> InsertRejectionDetails([FromBody]IEnumerable<RejectionModel> request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions< IEnumerable<InsertRejection>, IEnumerable<RejectionModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await rejectionService.AddAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("rejection-list")]
    public async Task<IActionResult> RejectionList([FromBody] ListsRequest request)
    {
        var result = await rejectionService.GetAllAsync(request);
        return Ok(result);
    }
    #endregion
}
