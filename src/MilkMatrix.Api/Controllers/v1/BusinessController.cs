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
public class BusinessController : ControllerBase
{
    private readonly IBusinessService businessService;
    private readonly IMapper mapper;
    private ILogging logging;

    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessController"/> class.
    /// </summary>
    /// <param name="businessService"></param>
    public BusinessController(IBusinessService businessService, IMapper mapper, ILogging logging, IHttpContextAccessor httpContextAccessor)
    {
        this.businessService = businessService;
        this.mapper = mapper;
        this.logging = logging.ForContext("ServiceName", nameof(BusinessController));
        this.httpContextAccessor = httpContextAccessor;
    }
    /// <summary>
    /// Retrieves the details of a business by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the business.</param>
    /// <returns>The details of the business.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            logging.LogInfo($"Get businessdetails by id called for id: {id}");
            var businessDetails = await businessService.GetByIdAsync(id);
            if (businessDetails == null)
            {
                logging.LogInfo($"businessdetails with id {id} not found.");
                return NoContent();
            }
            logging.LogInfo($"v with id {id} retrieved successfully.");
            return Ok(businessDetails);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving businessDetails with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the businessDetails.");
        }
    }

    [HttpPost("insert")]
    public async Task<IActionResult> InsertBusinessDetails([FromBody] BusinessInsertModel request)
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

            logging.LogInfo($"Upsert: Add called for businessdetails: {request.Name}");
            var requestParams = mapper.MapWithOptions<BusinessInsertRequest, BusinessInsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await businessService.AddAsync(requestParams);
            logging.LogInfo($"businessdetails {request.Name} added successfully.");
            return Ok(new { message = "businessdetails added successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert businessdetails", ex);
            return StatusCode(500, "An error occurred while processing the businessdetails.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateBusinessDetails([FromBody] BusinessUpdateModel request)
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

            logging.LogInfo($"Upsert: Update called for businessDetails: {request.Name}");
            var requestParams = mapper.MapWithOptions<BusinessUpdateRequest, BusinessUpdateModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await businessService.UpdateAsync(requestParams);
            logging.LogInfo($"businessDetails with {request.Name} updated successfully.");
            return Ok(new { message = "businessDetails updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert businessDetails", ex);
            return StatusCode(500, "An error occurred while processing the businessDetails.");
        }
    }


    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await businessService.GetAllAsync(request);
        return Ok(result);
    }

    [HttpPost("business-data")]
    public async Task<IActionResult> ListBusinessData([FromBody] BusinessDataModel request)
    {
        try
        {
            logging.LogInfo($"Get businessdetails by id called for id: {request.ActionType}");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<BusinessDataRequest, BusinessDataModel>(request
              , new Dictionary<string, object> {
            { Constants.AutoMapper.LoginId ,Convert.ToInt32(UserId)}
          });
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            var result = await businessService.GetBusinessDataByUserIdAsync(mapper.Map<BusinessDataRequest>(request));
            if (result == null || !result.Any())
            {
                logging.LogInfo($"businessdetails with id {request.ActionType} not found.");
                return NoContent();
            }
            logging.LogInfo($"business with id {request.ActionType} retrieved successfully.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving businessDetails with id: {request.UserId}", ex);
            return StatusCode(500, "An error occurred while retrieving the businessDetails.");
        }
    }
}
