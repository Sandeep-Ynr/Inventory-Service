using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Admin.Models.Admin.Responses.RolePage;
using MilkMatrix.Api.Models.Request.Admin.RolePage;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolePageController : ControllerBase
{
    private readonly IRolePageService rolePageService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public RolePageController(IRolePageService rolePageService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.rolePageService = rolePageService;
        this.logger = logger.ForContext("ServiceName", nameof(RolePageController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> InsertRolePage([FromBody] RolePageUpsertModel request)
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
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            logger.LogInfo($"Upsert: Add called for rolePage: {request.RoleId}");
            var requestParams = mapper.MapWithOptions<RolePageInsertRequest, RolePageUpsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await rolePageService.AddAsync(requestParams);
            logger.LogInfo($"RolePage {request.RoleId} added successfully.");
            return Ok(new { message = "rolePage added successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert role", ex);
            return StatusCode(500, "An error occurred while processing the role.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateRolePage([FromBody] RolePageUpsertModel request)
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

            logger.LogInfo($"Upsert: Update called for rolePage id: {request.RoleId}");
            var requestParams = mapper.MapWithOptions<RolePageUpdateRequest, RolePageUpsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await rolePageService.UpdateAsync(requestParams);
            logger.LogInfo($"rolePage with id {request.RoleId} updated successfully.");
            return Ok(new { message = "rolePage updated successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert role", ex);
            return StatusCode(500, "An error occurred while processing the role.");
        }
    }

    [HttpDelete("delete/{roleId}/{businessId}")]
    public async Task<IActionResult> Delete(int roleId, int businessId)
    {
        try
        {
            logger.LogInfo($"Delete rolepage called for id: {roleId}, {businessId}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await rolePageService.DeleteAsync(roleId, businessId, Convert.ToInt32(UserId));
            logger.LogInfo($"Rolepage with id {roleId}, {businessId} deleted successfully.");
            return Ok(new { message = "RolePage deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting RolePage with id: {roleId}, {businessId}", ex);
            return StatusCode(500, "An error occurred while deleting the RolePage.");
        }
    }

    [HttpGet("{roleId}/{businessId}")]
    public async Task<ActionResult<RolePages?>> GetById(int roleId, int businessId)
    {
        try
        {
            logger.LogInfo($"Get RolePage by id called for id: {roleId}");
            var page = await rolePageService.GetByIdAsync(roleId, businessId);
            if (!page.Any())
            {
                logger.LogInfo($"RolePage with id {roleId} not found.");
                return NotFound();
            }
            logger.LogInfo($"RolePage with id {roleId} retrieved successfully.");
            return Ok(page);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving RolePage with id: {roleId}", ex);
            return StatusCode(500, "An error occurred while retrieving the RolePage.");
        }
    }
}
