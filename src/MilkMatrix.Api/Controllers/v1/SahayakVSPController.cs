using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.SahayakVSP;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Logistics.Route;
using MilkMatrix.Milk.Contracts.SahayakVSP;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.SahayakVSP;
using MilkMatrix.Milk.Models.Response.SahayakVSP;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SahayakVSPController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly ISahayakVSPService sahayakvpsservice;
        public SahayakVSPController(IHttpContextAccessor httpContextAccessor, ILogging logging, IRouteService routeService, 
            IVehicleTypeService vehicleTypeService, ISahayakVSPService sahayakvpsservice, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.sahayakvpsservice = sahayakvpsservice ?? throw new ArgumentNullException(nameof(sahayakvpsservice));
            this.mapper = mapper;
        }

        #region SahayakVSP

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await sahayakvpsservice.GetAll(request);
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<SahayakVSPResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get by ID called for Sahayak ID: {id}");
                var result = await sahayakvpsservice.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"SahayakVSP with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"SahayakVSP with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving SahayakVSP with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record.");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SahayakVSPInsertRequestModel request)
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
                logger.LogInfo($"Add called for SahayakVSP: {request.SahayakName}");

                var mappedRequest = mapper.MapWithOptions<SahayakVSPInsertRequest, SahayakVSPInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await sahayakvpsservice.AddSahayakVSP(mappedRequest);
                return Ok(new { message = "SahayakVSP added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add SahayakVSP", ex);
                return StatusCode(500, "An error occurred while adding the record.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SahayakVSPUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.SahayakID <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var mappedRequest = mapper.MapWithOptions<SahayakVSPUpdateRequest, SahayakVSPUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                    { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                });

            await sahayakvpsservice.UpdateSahayakVSP(mappedRequest);
            logger.LogInfo($"SahayakVSP with ID {request.SahayakID} updated successfully.");
            return Ok(new { message = "SahayakVSP updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await sahayakvpsservice.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"SahayakVSP with ID {id} deleted successfully.");
                return Ok(new { message = "SahayakVSP deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting SahayakVSP with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the record.");
            }
        }

        #endregion
    }
}
