using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Module;
using MilkMatrix.Api.Models.Request.Admin.Module;
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
public class ModuleController : ControllerBase
{
    private readonly IModuleService moduleService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public ModuleController(IModuleService moduleService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.moduleService = moduleService;
        this.logger = logger.ForContext("ServiceName", nameof(ModuleController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertModule([FromBody] ModuleUpsertModel request)
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
                logger.LogInfo($"Upsert: Update called for Module id: {request.Id}");
                var requestParams = mapper.MapWithOptions<ModuleUpdateRequest, ModuleUpsertModel>(request
                    , new Dictionary<string, object> {
    {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                });
                await moduleService.UpdateAsync(requestParams);
                logger.LogInfo($"Module with id {request.Id} updated successfully.");
                return Ok(new { message = "Module updated successfully." });
            }
            else
            {
                logger.LogInfo($"Upsert: Add called for Module: {request.Name}");
                var requestParams = mapper.MapWithOptions<ModuleInsertRequest, ModuleUpsertModel>(request
                    , new Dictionary<string, object> {
    { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await moduleService.AddAsync(requestParams);
                logger.LogInfo($"Module {request.Name} added successfully.");
                return Ok(new { message = "Module added successfully." });
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert Module", ex);
            return StatusCode(500, "An error occurred while processing the Module.");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            logger.LogInfo($"Delete module called for id: {id}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await moduleService.DeleteAsync(id, Convert.ToInt32(UserId));
            logger.LogInfo($"module with id {id} deleted successfully.");
            return Ok(new { message = "module deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting module with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the module.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            logger.LogInfo($"Get module by id called for id: {id}");
            var module = await moduleService.GetByIdAsync(id);
            if (module == null)
            {
                logger.LogInfo($"module with id {id} not found.");
                return NoContent();
            }
            logger.LogInfo($"module with id {id} retrieved successfully.");
            return Ok(module);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving module with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the module.");
        }
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await moduleService.GetAllAsync(request);
        return Ok(result);
    }
}
