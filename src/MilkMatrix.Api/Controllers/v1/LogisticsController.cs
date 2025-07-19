using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Logistics.Transporter;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Logistics.Transporter;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Response.Logistics.Transporter;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogisticsController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly ITransporterService transporterService;

        public LogisticsController(IHttpContextAccessor httpContextAccessor, ILogging logger, ITransporterService transporterService, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(LogisticsController)) ?? throw new ArgumentNullException(nameof(logger));
            this.transporterService = transporterService ?? throw new ArgumentNullException(nameof(transporterService));
            this.mapper = mapper;
        }
        #region Transport-Regional
        [HttpPost("list-transporter")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await transporterService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("transporter/{id}")]
        public async Task<ActionResult<TransporterResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for Transporter ID: {id}");
                var result = await transporterService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Transporter with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Transporter with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Transporter with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }

        [HttpPost("add-transporter")]
        public async Task<IActionResult> Add([FromBody] TransporterInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = ErrorMessage.InvalidRequest
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<TransporterInsertRequest, TransporterInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await transporterService.AddTransporter(mappedRequest);
                logger.LogInfo($"Transporter {request.TransporterName} added successfully.");
                return Ok(new { message = "Transporter added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding transporter", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }

        [HttpPut("update-transporter")]
        public async Task<IActionResult> Update([FromBody] TransporterUpdateRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = ErrorMessage.InvalidRequest
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<TransporterUpdateRequest, TransporterUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                    { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await transporterService.UpdateTransporter(mappedRequest);
                logger.LogInfo($"Transporter {request.TransporterID} updated successfully.");
                return Ok(new { message = "Transporter updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating transporter", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }

        [HttpDelete("delete/{transporterId}")]
        public async Task<IActionResult> Delete(string transporterId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await transporterService.Delete(transporterId, Convert.ToInt32(userId));
                logger.LogInfo($"Transporter with ID {transporterId} deleted successfully.");
                return Ok(new { message = "Transporter deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Transporter with ID: {transporterId}", ex);
                return StatusCode(500, "An error occurred while deleting the transporter. " + ex.Message);
            }
        }

        #endregion
    }
}
