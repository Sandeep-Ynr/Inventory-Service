using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Route;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Route;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Route;
using MilkMatrix.Milk.Models.Response.Route;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IRouteService routeService;
        public RouteController(IHttpContextAccessor httpContextAccessor, ILogging logging, IRouteService routeService, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            this.mapper = mapper;
        }
        #region Route

        [HttpPost("route-list")]
        public async Task<IActionResult> GetRouteList([FromBody] ListsRequest request)
        {
            var result = await routeService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("routeID{id}")]
        public async Task<ActionResult<RouteResponse?>> GetRouteById(int id)
        {
            try
            {
                logger.LogInfo($"Get Route by ID called for id: {id}");
                var result = await routeService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Route with ID {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Route with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Route with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Route.");
            }
        }

        [HttpPost("add-route")]
        public async Task<IActionResult> AddRoute([FromBody] RouteInsertRequestModel request)
        {
            try
            {
                if ((request == null) || (!ModelState.IsValid))
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid request."
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                logger.LogInfo($"Add called for Route: {request.Name}");

                var mappedRequest = mapper.MapWithOptions<RouteInsertRequest, RouteInsertRequestModel>(request,
                    new Dictionary<string, object> {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await routeService.AddRoute(mappedRequest);
                logger.LogInfo($"Route {request.Name} added successfully.");
                return Ok(new { message = "Route added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Route", ex);
                return StatusCode(500, "An error occurred while adding the Route.");
            }
        }

        [HttpPut("update-route")]
        public async Task<IActionResult> UpdateRoute([FromBody] RouteUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.RouteID <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var mappedRequest = mapper.MapWithOptions<RouteUpdateRequest, RouteUpdateRequestModel>(request,
                new Dictionary<string, object> {
                    { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                });

            await routeService.UpdateRoute(mappedRequest);
            logger.LogInfo($"Route with ID {request.RouteID} updated successfully.");
            return Ok(new { message = "Route updated successfully." });
        }

        [HttpDelete("route-delete/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await routeService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Route with ID {id} deleted successfully.");
                return Ok(new { message = "Route deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Route with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Route.");
            }
        }

        #endregion
    }
}
