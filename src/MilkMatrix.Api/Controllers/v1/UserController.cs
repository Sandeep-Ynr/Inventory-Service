using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Api.Models.Request.Admin.User;
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
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public UserController(IUserService userService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.userService = userService;
        this.logger = logger.ForContext("ServiceName", nameof(UserController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertUser([FromBody] UserUpsertModel request)
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
                logger.LogInfo($"Upsert: Update called for user id: {request.Id}");
                var requestParams = mapper.MapWithOptions<UserUpdateRequest, UserUpsertModel>(request
                    , new Dictionary<string, object> {
                { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                });
                await userService.UpdateAsync(requestParams);
                logger.LogInfo($"User with id {request.Id} updated successfully.");
                return Ok(new { message = "User updated successfully." });
            }
            else
            {
                logger.LogInfo($"Upsert: Add called for user: {request.Name}");
                var requestParams = mapper.MapWithOptions<UserInsertRequest, UserUpsertModel>(request
                    , new Dictionary<string, object> {
                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await userService.AddAsync(requestParams);
                logger.LogInfo($"User {request.Name} added successfully.");
                return Ok(new { message = "User added successfully." });
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert user", ex);
            return StatusCode(500, "An error occurred while processing the user.");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            logger.LogInfo($"Delete user called for id: {id}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await userService.DeleteAsync(id, Convert.ToInt32(UserId));
            logger.LogInfo($"User with id {id} deleted successfully.");
            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting user with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the user.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetails?>> GetById(int id)
    {
        try
        {
            logger.LogInfo($"Get user by id called for id: {id}");
            var user = await userService.GetByIdAsync(id);
            if (user == null)
            {
                logger.LogInfo($"User with id {id} not found.");
                return NotFound();
            }
            logger.LogInfo($"User with id {id} retrieved successfully.");
            return Ok(user);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving user with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the user.");
        }
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var result = await userService.GetAllAsync(request,Convert.ToInt32(UserId));
        return Ok(result);
    }
}
