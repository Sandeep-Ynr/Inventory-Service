using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Business.Admin.Implementation;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Api.Models.Request.Admin.Page;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PageController : ControllerBase
{
    private readonly IPageService pageService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public PageController(IPageService pageService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.pageService = pageService;
        this.logger = logger.ForContext("ServiceName", nameof(PageController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertRole([FromBody] PageUpsertModel request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            // If Id is present and > 0, treat as update, else add
            if (request.Id != null && request.Id > 0)
            {
                logger.LogInfo($"Upsert: Update called for role id: {request.Id}");
                var requestParams = mapper.MapWithOptions<PageUpdateRequest, PageUpsertModel>(request
                    , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                });
                await pageService.UpdateAsync(requestParams);
                logger.LogInfo($"Page with id {request.Id} updated successfully.");
                return Ok(new { message = "role updated successfully." });
            }
            else
            {
                logger.LogInfo($"Upsert: Add called for page: {request.Name}");
                var requestParams = mapper.MapWithOptions<PageInsertRequest, PageUpsertModel>(request
                    , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await pageService.AddAsync(requestParams);
                logger.LogInfo($"Page {request.Name} added successfully.");
                return Ok(new { message = "Page added successfully." });
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert role", ex);
            return StatusCode(500, "An error occurred while processing the role.");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            logger.LogInfo($"Delete page called for id: {id}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await pageService.DeleteAsync(id, Convert.ToInt32(UserId));
            logger.LogInfo($"page with id {id} deleted successfully.");
            return Ok(new { message = "page deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting page with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the page.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pages?>> GetById(int id)
    {
        try
        {
            logger.LogInfo($"Get page by id called for id: {id}");
            var page = await pageService.GetByIdAsync(id);
            if (page == null)
            {
                logger.LogInfo($"page with id {id} not found.");
                return NoContent();
            }
            logger.LogInfo($"page with id {id} retrieved successfully.");
            return Ok(page);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving page with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the page.");
        }
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await pageService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of pages approval available.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("page-approval-list")]
    public async Task<IActionResult> GetPageApprovalList([FromQuery] int? id = null)
    {
        var response = await pageService.GetPagesForApprovalAsync(id);
        return response != null && response.Any()
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }
}
