using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Logistics.Route;
using MilkMatrix.Api.Models.Request.Logistics.Transporter;
using MilkMatrix.Api.Models.Request.Logistics.Vehicle;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Logistics.Route;
using MilkMatrix.Milk.Contracts.Logistics.Transporter;
using MilkMatrix.Milk.Contracts.Logistics.Vehicle;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Logistics.Route;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Request.Logistics.VehcileType;
using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Response.Logistics.Route;
using MilkMatrix.Milk.Models.Response.Logistics.Transporter;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleType;
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
        private readonly IRouteService routeService;
        private readonly IVehicleTypeService vehicleTypeService;
        private readonly IVehicleService vehicleService;
        public LogisticsController(IHttpContextAccessor httpContextAccessor, ILogging logger,
            ITransporterService transporterService, IRouteService routeService, IVehicleTypeService vehicleTypeService
            , IVehicleService vehicleService, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(LogisticsController)) ?? throw new ArgumentNullException(nameof(logger));
            this.transporterService = transporterService ?? throw new ArgumentNullException(nameof(transporterService));
            this.routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            this.vehicleTypeService = vehicleTypeService ?? throw new ArgumentNullException(nameof(vehicleTypeService));
            this.vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            this.mapper = mapper;
        }
        #region Transport
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
        public async Task<IActionResult> Delete(int transporterId)
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
            //hi//
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
        #region Vehicle-Type

        [HttpPost("vehicleType-list")]
        public async Task<IActionResult> VehicleTypeList([FromBody] ListsRequest request)
        {
            var result = await vehicleTypeService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("vehicleTypeID{id}")]
        public async Task<ActionResult<VehicleTypeResponse?>> GetVehicleTypeById(int id)
        {
            try
            {
                logger.LogInfo($"Get Vehicle Type by id called for id: {id}");
                var result = await vehicleTypeService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Vehicle Type with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Vehicle Type with id {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Vehicle Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Vehicle Type.");
            }
        }

        [HttpPost("add-vehicleType")]
        public async Task<IActionResult> AddVehicleType([FromBody] VehicleTypeInsertRequestModel request)
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
                logger.LogInfo($"Add called for Vehicle Type: {request.VehicleType}");

                var requestParams = mapper.MapWithOptions<VehicleTypeInsertRequest, VehicleTypeInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                     { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await vehicleTypeService.AddVehicleType(requestParams);

                logger.LogInfo($"Vehicle Type {request.VehicleType} added successfully.");
                return Ok(new { message = "Vehicle Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in AddVehicleType", ex);
                return StatusCode(500, "An error occurred while adding the Vehicle Type.");
            }
        }

        [HttpPut("update-vehicleType")]
        public async Task<IActionResult> UpdateVehicleType([FromBody] VehicleTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.VehicleID <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<VehicleTypeUpdateRequest, VehicleTypeUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                 { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                });

            await vehicleTypeService.UpdateVehicleType(requestParams);

            logger.LogInfo($"Vehicle Type with id {request.VehicleID} updated successfully.");
            return Ok(new { message = "Vehicle Type updated successfully." });
        }

        [HttpDelete("vehicleType-delete/{id}")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await vehicleTypeService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Vehicle Type with id {id} deleted successfully.");
                return Ok(new { message = "Vehicle Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Vehicle Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Vehicle Type.");
            }
        }

        #endregion

        #region Vehicle

        [HttpPost("vehicle-list")]
        public async Task<IActionResult> VehicleList([FromBody] ListsRequest request)
        {
            var result = await vehicleService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("vehicleID{id}")]
        public async Task<ActionResult<VehicleTypeResponse?>> GetVehicleById(int id)
        {
            try
            {
                logger.LogInfo($"Get Vehicle by id called for id: {id}");
                var result = await vehicleService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Vehicle with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Vehicle with id {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Vehicle with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Vehicle");
            }
        }

        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleInsertRequestModel request)
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
                logger.LogInfo($"Add called for Vehicle Type: {request.RegistrationNo}");

                var requestParams = mapper.MapWithOptions<VehicleInsertRequest, VehicleInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                     { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await vehicleService.AddVehicle(requestParams);

                logger.LogInfo($"Vehicle Type {request.RegistrationNo} added successfully.");
                return Ok(new { message = "Vehicl added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in AddVehicle", ex);
                return StatusCode(500, "An error occurred while adding the Vehicle");
            }
        }

        [HttpPut("update-vehicle")]
        public async Task<IActionResult> UpdateVehicle([FromBody] VehicleTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.VehicleID <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<VehicleTypeUpdateRequest, VehicleTypeUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                 { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                });

            await vehicleTypeService.UpdateVehicleType(requestParams);

            logger.LogInfo($"Vehicle Type with id {request.VehicleID} updated successfully.");
            return Ok(new { message = "Vehicle Type updated successfully." });
        }

        [HttpDelete("vehicle-delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await vehicleTypeService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Vehicle Type with id {id} deleted successfully.");
                return Ok(new { message = "Vehicle Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Vehicle Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Vehicle Type.");
            }
        }

        #endregion

    }
}
