using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Api.Models.Request.Admin.Role;
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
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public RoleController(IRoleService roleService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.roleService = roleService;
        this.logger = logger.ForContext("ServiceName", nameof(RoleController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertRole([FromBody] RoleUpsertModel request)
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
                var requestParams = mapper.MapWithOptions<RoleUpdateRequest, RoleUpsertModel>(request
                    , new Dictionary<string, object> {
            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                });
                await roleService.UpdateAsync(requestParams);
                logger.LogInfo($"Role with id {request.Id} updated successfully.");
                return Ok(new { message = "role updated successfully." });
            }
            else
            {
                logger.LogInfo($"Upsert: Add called for role: {request.Name}");
                var requestParams = mapper.MapWithOptions<RoleInsertRequest, RoleUpsertModel>(request
                    , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await roleService.AddAsync(requestParams);
                logger.LogInfo($"role {request.Name} added successfully.");
                return Ok(new { message = "Role added successfully." });
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
            logger.LogInfo($"Delete role called for id: {id}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await roleService.DeleteAsync(id, Convert.ToInt32(UserId));
            logger.LogInfo($"role with id {id} deleted successfully.");
            return Ok(new { message = "role deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting role with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the role.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            logger.LogInfo($"Get role by id called for id: {id}");
            var role = await roleService.GetByIdAsync(id);
            if (role == null)
            {
                logger.LogInfo($"role with id {id} not found.");
                return NoContent();
            }
            logger.LogInfo($"role with id {id} retrieved successfully.");
            return Ok(role);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving role with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the role.");
        }
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await roleService.GetAllAsync(request);
        return Ok(result);
    }
}
