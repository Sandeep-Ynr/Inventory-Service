using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.MPP;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.MPP;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.MPP;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MPPController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IMPPService mppService;
        public MPPController(IHttpContextAccessor httpContextAccessor, ILogging logging, IMPPService mppService, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.mppService = mppService ?? throw new ArgumentNullException(nameof(mppService));
            this.mapper = mapper;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await mppService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<MPPResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for MPP ID: {id}");
                var result = await mppService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"MPP with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"MPP with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MPP with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record." + ex);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] MPPInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid request."
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                logger.LogInfo($"Add called for MPP: {request.MPPName}");

                var mappedRequest = mapper.MapWithOptions<MPPInsertRequest, MPPInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await mppService.AddMPP(mappedRequest);
                return Ok(new { message = "MPP added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add MPP", ex);
                return StatusCode(500, $"An error occurred while adding the MPP. {ex.Message}");

            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] MPPUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.MPPID <= 0)
                    return BadRequest("Invalid request.");

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<MPPUpdateRequest, MPPUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                    { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await mppService.UpdateMPP(mappedRequest);
                logger.LogInfo($"MPP with ID {request.MPPID} updated successfully.");
                return Ok(new { message = "MPP updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in updating MPP", ex);
                return StatusCode(500, $"An error occurred while updating the MPP. {ex.Message}");
            }

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await mppService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"MPP with ID {id} deleted successfully.");
                return Ok(new { message = "MPP deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting MPP with ID: {id}", ex);
                return StatusCode(500, $"An error occurred while deleting the MPP. {ex.Message}");
            }
        }
    }


}

