using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Requests.SubModule;
using MilkMatrix.Api.Models.Request.Admin.Role;
using MilkMatrix.Api.Models.Request.Admin.SubModule;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SubModuleController : ControllerBase
{
    private readonly ISubModuleService subModuleService;

    private ILogging logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public SubModuleController(ISubModuleService subModuleService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        this.subModuleService = subModuleService;
        this.logger = logger.ForContext("ServiceName", nameof(SubModuleController));
        this.httpContextAccessor = httpContextAccessor;
        this.mapper = mapper;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertSubModule([FromBody] SubModuleUpsertModel request)
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
                logger.LogInfo($"Upsert: Update called for subModule id: {request.Id}");
                var requestParams = mapper.MapWithOptions<SubModuleUpdateRequest, SubModuleUpsertModel>(request
                    , new Dictionary<string, object> {
        {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                });
                await subModuleService.UpdateAsync(requestParams);
                logger.LogInfo($"SubModule with id {request.Id} updated successfully.");
                return Ok(new { message = "subModule updated successfully." });
            }
            else
            {
                logger.LogInfo($"Upsert: Add called for subModule: {request.Name}");
                var requestParams = mapper.MapWithOptions<SubModuleInsertRequest, SubModuleUpsertModel>(request
                    , new Dictionary<string, object> {
        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await subModuleService.AddAsync(requestParams);
                logger.LogInfo($"subModule {request.Name} added successfully.");
                return Ok(new { message = "SubModule added successfully." });
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error in Upsert subModule", ex);
            return StatusCode(500, "An error occurred while processing the subModule.");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            logger.LogInfo($"Delete subModule called for id: {id}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await subModuleService.DeleteAsync(id, Convert.ToInt32(UserId));
            logger.LogInfo($"subModule with id {id} deleted successfully.");
            return Ok(new { message = "subModule deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting subModule with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the subModule.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            logger.LogInfo($"Get subModule by id called for id: {id}");
            var subModule = await subModuleService.GetByIdAsync(id);
            if (subModule == null)
            {
                logger.LogInfo($"subModule with id {id} not found.");
                return NoContent();
            }
            logger.LogInfo($"subModule with id {id} retrieved successfully.");
            return Ok(subModule);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving subModule with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the subModule.");
        }
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await subModuleService.GetAllAsync(request);
        return Ok(result);
    }

}
