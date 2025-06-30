using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Api.Models.Request.Admin.RolePage;
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

    [HttpGet("{roleId}")]
    public async Task<ActionResult> GetById(int roleId)
    {
        try
        {
            logger.LogInfo($"Get RolePage by id called for id: {roleId}");
            var page = await rolePageService.GetByIdAsync(roleId);
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

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await rolePageService.GetAllAsync(request);
        return Ok(result);
    }
}
