using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Shift;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Shift;
using MilkMatrix.Milk.Implementations.Shift;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Shift;
using MilkMatrix.Milk.Models.Response.Shift;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IShiftService shiftService;

        public ShiftController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IShiftService shiftService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.shiftService = shiftService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await shiftService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("Shift{id}")]
        public async Task<ActionResult<ShiftInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Shift by id called for id: {id}");
                var mcc = await shiftService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Shift with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Shift with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Shift with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Shift.");
            }
        }

        [HttpPost]
        [Route("add-shift")]
        public async Task<IActionResult> AddAsync([FromBody] ShiftInsertRequestModel request)
        {
            try
            {
                if ((request == null) || (!ModelState.IsValid))
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                    });
                }
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                logger.LogInfo($"Add called for Shift: {request.ShiftName}");
                var requestParams = mapper.MapWithOptions<ShiftInsertRequest, ShiftInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await shiftService.AddAsync(requestParams);
                logger.LogInfo($"Shift {request.ShiftName} added successfully.");
                return Ok(new { message = "Shift added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Shift", ex);
                return StatusCode(500, "An error occurred while adding the Shift.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ShiftUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<ShiftUpdateRequest, ShiftUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await shiftService.UpdateAsync(requestParams);
            logger.LogInfo($"Shift with id {request.ShiftId} updated successfully.");
            return Ok(new { message = "Shift updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await shiftService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Shift with id {id} deleted successfully.");
                return Ok(new { message = "Shift deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Shift with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Shift.");
            }
        }

    }
}
