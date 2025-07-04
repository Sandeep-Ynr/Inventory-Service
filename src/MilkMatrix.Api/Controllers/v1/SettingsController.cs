using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;
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
public class SettingsController : ControllerBase
{
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logging;
    private readonly IConfigurationService configService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsController"/> class.
    /// </summary>
    /// <param name="ihttpContextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="logging"></param>
    /// <param name="configService"></param>
    public SettingsController(IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logging, IConfigurationService configService)
    {
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.logging = logging.ForContext("ServiceName", nameof(SettingsController));
        this.configService = configService;
    }

    #region Configuration Settings

    /// <summary>
    /// Retrieves the details of a configuration setting or tag by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("config/{id}")]
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
    [HttpPost("config-insert")]
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
    [HttpPut("config-update")]
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
    [HttpPost("config-list")]
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
    [HttpDelete("config-delete/{id}")]
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

    #region Email Settings

    /// <summary>
    /// Retrieves the details of a Smtp setting by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("smtp/{id}")]
    public async Task<ActionResult> GetBySmtpId(int id)
    {
        try
        {
            logging.LogInfo($"Get SmtpDetails by id called for id: {id}");
            var configDetails = await configService.GetBySmtpIdAsync(id);
            if (configDetails == null)
            {
                logging.LogInfo($"SmtpDetails with id {id} not found.");
                return NoContent();
            }
            logging.LogInfo($"SmtpDetails with id {id} retrieved successfully.");
            return Ok(configDetails);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving SmtpDetails with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the SmtpDetails.");
        }
    }

    /// <summary>
    /// Inserts a new smtp setting into the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("smtp-insert")]
    public async Task<IActionResult> InsertSmtpDetails([FromBody] SmtpSettingsInsertModel request)
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

            logging.LogInfo($"Upsert: Add called for SMtpDetails: {request.SmtpServer}");
            var requestParams = mapper.MapWithOptions<SmtpSettingsInsert, SmtpSettingsInsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await configService.AddSmtpDetailsAsync(requestParams);
            logging.LogInfo($"SmtpDetails {request.SmtpServer} added successfully.");
            return Ok(new { message = "SmtpDetails added successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert ConfigDetails", ex);
            return StatusCode(500, "An error occurred while processing the SmtpDetails.");
        }
    }

    /// <summary>
    /// Updates an existing configuration setting or tag in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("smtp-update")]
    public async Task<IActionResult> UpdateSmtpDetails([FromBody] SmtpSettingsUpdateModel request)
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

            logging.LogInfo($"Upsert: Update called for SmtpDetails: {request.SmtpServer}");
            var requestParams = mapper.MapWithOptions<SmtpSettingsUpdate, SmtpSettingsUpdateModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await configService.UpdateSmtpDetailsAsync(requestParams);
            logging.LogInfo($"SmtpDetails with {request.SmtpServer} updated successfully.");
            return Ok(new { message = "SmtpDetails updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Upsert SmtpDetails", ex);
            return StatusCode(500, "An error occurred while processing the SmtpDetails.");
        }
    }

    /// <summary>
    /// Retrieves a list of configuration settings or tags from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("smtp-list")]
    public async Task<IActionResult> SmtpList([FromBody] ListsRequest request)
    {
        var result = await configService.GetAllSmtpDetaisAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a configuration setting or tag from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("smtp-delete/{id}")]
    public async Task<IActionResult> DeleteSmtp(int id)
    {
        try
        {
            logging.LogInfo($"Delete smtp called for id: {id}");
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await configService.DeleteSmtpDetailsAsync(id, Convert.ToInt32(UserId));
            logging.LogInfo($"SmtpDetails with id {id} deleted successfully.");
            return Ok(new { message = "SmtpDetails deleted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error deleting Smtpdetails with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the Smtp.");
        }
    }
    #endregion

    #region Blocked Mobile

    /// <summary>
    /// Retrieves the details of a Blocked mobiles setting by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("block-mobile/{id}")]
    public async Task<ActionResult> GetBlockedMobilesById(int id)
    {
        try
        {
            logging.LogInfo($"Get details by id called for id: {id}");
            var configDetails = await configService.GetBlockedMobilesAsync(id);
            if (configDetails == null)
            {
                logging.LogInfo($"details with id {id} not found.");
                return NoContent();
            }
            logging.LogInfo($"Details with id {id} retrieved successfully.");
            return Ok(configDetails);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving Details with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the Details.");
        }
    }

    /// <summary>
    /// Inserts a new mobile to be blocked into the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("block-mobile-insert")]
    public async Task<IActionResult> InsertBlockedMobileDetails([FromBody] BlockedMobileInsertModel request)
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

            var requestParams = mapper.MapWithOptions<BlockedMobilesInsert, BlockedMobileInsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await configService.AddMobileBlockAsync(requestParams);
            logging.LogInfo($"details {request.MobileNumber} added successfully.");
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in InsertBlockedMobileDetails", ex);
            return StatusCode(500, "An error occurred while adding details.");
        }
    }

    /// <summary>
    /// Updates an existing blocked mobile in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("block-mobile-update")]
    public async Task<IActionResult> UpdateBlockedMobileDetails([FromBody] BlockedMobileUpdateModel request)
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

            var requestParams = mapper.MapWithOptions<BlockedMobilesUpdate, BlockedMobileUpdateModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await configService.UpdateMobileBlockAsync(requestParams);
            logging.LogInfo($"Details with {request.MobileNumber} updated successfully.");
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Updating the blockedMobile details", ex);
            return StatusCode(500, "An error occurred while updating the details.");
        }
    }

    /// <summary>
    /// Retrieves a list of mobiles blocked from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("blocked-list")]
    public async Task<IActionResult> GetBlockedMobiles([FromBody] ListsRequest request)
    {
        var result = await configService.GetAllBlockedMobilesAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a blocked mobile from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("block-mobile-delete/{id}")]
    public async Task<IActionResult> DeleteBlockedMobile(int id)
    {
        try
        {
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await configService.DeleteMobileBlockAsync(id, Convert.ToInt32(UserId));
            logging.LogInfo($"details with id {id} deleted successfully.");
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error deleting with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting.");
        }
    }
    #endregion

    #region Sms Settings

    /// <summary>
    /// Retrieves the details of a Sms setting by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("sms-control/{id}")]
    public async Task<ActionResult> GetBySmsControlId(int id)
    {
        try
        {
            var configDetails = await configService.GetBySmsControlByIdAsync(id);
            if (configDetails == null)
            {
                logging.LogInfo($"Details with id {id} not found.");
                return NoContent();
            }
            logging.LogInfo($"Details with id {id} retrieved successfully.");
            return Ok(configDetails);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving Details with id: {id}", ex);
            return StatusCode(500, "An error occurred while retrieving the Details.");
        }
    }

    /// <summary>
    /// Inserts a new sms setting into the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("sms-control-insert")]
    public async Task<IActionResult> InsertSmsControlDetails([FromBody] SmsControlInsertModel request)
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

            var requestParams = mapper.MapWithOptions<SmsControlInsert, SmsControlInsertModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await configService.AddSmsControlDetailsAsync(requestParams);
            logging.LogInfo($"SmsMerchant {request.SmsMerchant} added successfully.");
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert MerchantDetails", ex);
            return StatusCode(500, "An error occurred while processing the MerchantDetails.");
        }
    }

    /// <summary>
    /// Updates an existing sms setting in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("sms-control-update")]
    public async Task<IActionResult> UpdateSmsControlDetails([FromBody] SmsControlUpdateModel request)
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

            var requestParams = mapper.MapWithOptions<SmsControlUpdate, SmsControlUpdateModel>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
            });
            await configService.UpdateSmsDetailsAsync(requestParams);
            logging.LogInfo($"Sms control with {request.SmsMerchant} updated successfully.");
            return Ok(new { message = "Sms control updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Update Sms control", ex);
            return StatusCode(500, "An error occurred while updating the Sms control.");
        }
    }

    /// <summary>
    /// Retrieves a list of sms control from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("sms-control-list")]
    public async Task<IActionResult> SmsControlList([FromBody] ListsRequest request)
    {
        var result = await configService.GetAllSmsDetaisAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a sms control from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("sms-control-delete/{id}")]
    public async Task<IActionResult> DeleteSmsControl(int id)
    {
        try
        {
            logging.LogInfo($"Delete Sms control called for id: {id}");
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await configService.DeleteSmsDetailsAsync(id, Convert.ToInt32(UserId));
            logging.LogInfo($"Sms control with id {id} deleted successfully.");
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error deleting sms control with id: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the Sms control.");
        }
    }
    #endregion
}
