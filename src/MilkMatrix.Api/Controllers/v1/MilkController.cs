using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Milk;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Milk;
//using MilkMatrix.Milk.Implementations.Milk;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Milk;
using MilkMatrix.Milk.Models.Response.Milk;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class MilkController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IMilkService milkService;

        public MilkController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IMilkService milkService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.milkService = milkService;
        }

        [HttpPost]
        [Route("milktypelist")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await milkService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("milktype{id}")]
        public async Task<ActionResult<MilkTypeInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Milk Type by id called for id: {id}");
                var mcc = await milkService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Milk Type with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Milk Type with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Milk Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Milk Type.");
            }
        }

        [HttpPost]
        [Route("addmilktype")]
        public async Task<IActionResult> AddAsync([FromBody] MilkTypeInsertRequestModel request)
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

                logger.LogInfo($"Add called for Animal Type: {request.MilkTypeName}");
                var requestParams = mapper.MapWithOptions<MilkTypeInsertRequest, MilkTypeInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await milkService.AddAsync(requestParams);
                logger.LogInfo($"Animal Type {request.MilkTypeName} added successfully.");
                return Ok(new { message = "Milk Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Animal Type", ex);
                return StatusCode(500, "An error occurred while adding the Milk Type.");
            }
        }

        [HttpPut]
        [Route("updatemilktype/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MilkTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<MilkTypeUpdateRequest, MilkTypeUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await milkService.UpdateAsync(requestParams);
            logger.LogInfo($"Milk Type with id {request.MilkTypeId} updated successfully.");
            return Ok(new { message = "Milk Type updated successfully." });
        }

        [HttpDelete("deletemilktype/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await milkService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Milk Type with id {id} deleted successfully.");
                return Ok(new { message = "Milk Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Milk Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Milk Type.");
            }
        }

    }
}
