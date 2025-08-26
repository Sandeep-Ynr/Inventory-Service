using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Mcc;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Mcc;
using MilkMatrix.Milk.Implementations;

using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Mcc;
using MilkMatrix.Milk.Models.Response.Mcc;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MccController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IMccService mccService;

        public MccController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IMccService mccService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.mccService = mccService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await mccService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("mcc{id}")]
        public async Task<ActionResult<MccIndividualResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get MCC by id called for id: {id}");
                var mcc = await mccService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"MCC with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"MCC with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MCC with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the MCC.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddMccAsync([FromBody] MccInsertRequestModel request)
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

                logger.LogInfo($"Add called for MCC: {request.MccName}");
                var requestParams = mapper.MapWithOptions<MccInsertRequest, MccInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await mccService.AddAsync(requestParams);
                logger.LogInfo($"MCC {request.MccName} added successfully.");
                return Ok(new { message = "MCC added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add MCC", ex);
                return StatusCode(500, "An error occurred while adding the MCC.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateMccAsync(int id, [FromBody] MccUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<MccUpdateRequest, MccUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await mccService.UpdateAsync(requestParams);
            logger.LogInfo($"MCC with id {request.MccId} updated successfully.");
            return Ok(new { message = "MCC updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await mccService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"MCC with id {id} deleted successfully.");
                return Ok(new { message = "MCC deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting MCC with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the MCC.");
            }
        }

    }
}
