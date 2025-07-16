using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Bmc;
using MilkMatrix.Api.Models.Request.Mcc;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;

using MilkMatrix.Milk.Contracts.Bmc;
using MilkMatrix.Milk.Implementations.Bmc;

using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Bmc;
using MilkMatrix.Milk.Models.Request.Mcc;
using MilkMatrix.Milk.Models.Response.Bmc;
using MilkMatrix.Milk.Models.Response.Mcc;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BmcController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IBmcService bmcService;

        public BmcController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IBmcService bmcService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.bmcService = bmcService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await bmcService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("bmc{id}")]
        public async Task<ActionResult<BmcIndividualResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get BMC by id called for id: {id}");
                var mcc = await bmcService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"BMC with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"BMC with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving BMC with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the BMC.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddBmcAsync([FromBody] BmcInsertRequestModel request)
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

                logger.LogInfo($"Add called for BMC: {request.BmcName}");
                var requestParams = mapper.MapWithOptions<BmcInsertRequest, BmcInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await bmcService.AddAsync(requestParams);
                logger.LogInfo($"BMC {request.BmcName} added successfully.");
                return Ok(new { message = "BMC added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add BMC", ex);
                return StatusCode(500, "An error occurred while adding the BMC.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBmcAsync(int id, [FromBody] BmcUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<BmcUpdateRequest, BmcUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await bmcService.UpdateAsync(requestParams);
            logger.LogInfo($"BMC with id {request.BmcId} updated successfully.");
            return Ok(new { message = "BMC updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await bmcService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"BMC with id {id} deleted successfully.");
                return Ok(new { message = "MCC deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting BMC with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the BMC.");
            }
        }


    }
}
