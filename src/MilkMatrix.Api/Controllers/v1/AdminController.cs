using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Business.Admin.Implementation;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using static Azure.Core.HttpHeader;
using static MilkMatrix.Api.Common.Constants.Constants;

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
    private readonly IAuth iAuthentication;
    private readonly ICommonModules commonModules;
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logging;
    private readonly IConfigurationService configService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="iAuthentication"></param>
    /// <param name="ihttpContextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="logging"></param>
    /// <param name="commonModules"></param>
    /// <param name="configService"></param>   
    public AdminController(IAuth iAuthentication, IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logging, ICommonModules commonModules, IConfigurationService configService)
    {
        this.iAuthentication = iAuthentication;
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.commonModules = commonModules;
        this.logging = logging.ForContext("ServiceName", nameof(AdminController));
        this.configService = configService;
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


    #region Configuration Settings

    /// <summary>
    /// Retrieves the details of a configuration setting or tag by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            logging.LogInfo($"Get ConfigDetails by id called for id: {id}");
            var configDetails = await configService.GetByIdAsync(id);
            if (configDetails == null)
            {
                logging.LogInfo($"ConfigDetails with id {id} not found.");
                return NoContent();
            }
            logging.LogInfo($"ConfigDetails with id {id} retrieved successfully.");
            return Ok(configDetails);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving ConfigDetails with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the ConfigDetails.");
        }
    }

    /// <summary>
    /// Inserts a new configuration setting or tag into the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("insert")]
    public async Task<IActionResult> InsertConfigDetails([FromBody] ConfigurationInsertModel request)
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

            logging.LogInfo($"Upsert: Add called for ConfigDetails: {request.TagName}");
            var requestParams = mapper.MapWithOptions<ConfigurationInsertRequest, ConfigurationInsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await configService.AddAsync(requestParams);
            logging.LogInfo($"ConfigDetails {request.TagName} added successfully.");
            return Ok(new { message = "ConfigDetails added successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert ConfigDetails", ex);
            return StatusCode(500, "An error occurred while processing the ConfigDetails.");
        }
    }

    /// <summary>
    /// Updates an existing configuration setting or tag in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<IActionResult> UpdateConfigDetails([FromBody] ConfigurationUpdateModel request)
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
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            logging.LogInfo($"Upsert: Update called for ConfigDetails: {request.TagName}");
            var requestParams = mapper.MapWithOptions<ConfigurationUpdateRequest, ConfigurationUpdateModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await configService.UpdateAsync(requestParams);
            logging.LogInfo($"ConfigDetails with {request.TagName} updated successfully.");
            return Ok(new { message = "ConfigDetails updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert ConfigDetails", ex);
            return StatusCode(500, "An error occurred while processing the ConfigDetails.");
        }
    }

    /// <summary>
    /// Retrieves a list of configuration settings or tags from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("list")]
    public async Task<IActionResult> ConfigList([FromBody] ListsRequest request)
    {
        var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var result = await configService.GetAllAsync(request, Convert.ToInt32(UserId));
        return Ok(result);
    }

    /// <summary>
    /// Deletes a configuration setting or tag from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            logging.LogInfo($"Delete configuration called for id: {id}");
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await configService.DeleteAsync(id, Convert.ToInt32(UserId));
            logging.LogInfo($"Configuration with id {id} deleted successfully.");
            return Ok(new { message = "Configuration deleted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error deleting configuration with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the configuration.");
        }
    }
    #endregion
}
