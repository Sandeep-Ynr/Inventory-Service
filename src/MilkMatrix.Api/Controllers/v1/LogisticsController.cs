using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Logistics.Route;
using MilkMatrix.Api.Models.Request.Logistics.Transporter;
using MilkMatrix.Api.Models.Request.Logistics.Vehicle;
using MilkMatrix.Api.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Api.Models.Request.Logistics.Vendor;
using MilkMatrix.Api.Models.Request.Route.RouteContractor;
using MilkMatrix.Api.Models.Request.Route.RouteTiming;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Logistics.Route;
using MilkMatrix.Milk.Contracts.Logistics.Transporter;
using MilkMatrix.Milk.Contracts.Logistics.Vehicle;
using MilkMatrix.Milk.Contracts.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Contracts.Logistics.Vendor;
using MilkMatrix.Milk.Contracts.Route.RouteContractor;
using MilkMatrix.Milk.Contracts.Route.RouteTiming;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Implementations.Route.RouteContractor;
using MilkMatrix.Milk.Implementations.Route.RouteTiming;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Logistics.Route;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Request.Logistics.VehcileType;
using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Request.Logistics.Vendor;
using MilkMatrix.Milk.Models.Request.Route.RouteContractor;
using MilkMatrix.Milk.Models.Request.Route.RouteTiming;
using MilkMatrix.Milk.Models.Response.Logistics.Route;
using MilkMatrix.Milk.Models.Response.Logistics.Transporter;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleType;
using MilkMatrix.Milk.Models.Response.Logistics.Vendor;
using MilkMatrix.Milk.Models.Response.Route.RouteTiming;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
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
        private readonly IVendorService vendorService;
        private readonly IVehicleBillingTypeService vehicleBillingTypeService;
        private readonly IRouteContractorService routeContractorService;
        private readonly IRouteTimingService routeTimingService;

        public LogisticsController(IHttpContextAccessor httpContextAccessor, ILogging logger,
            ITransporterService transporterService, IRouteService routeService, IVehicleTypeService vehicleTypeService
            , IVehicleService vehicleService, IVendorService vendorService, IVehicleBillingTypeService vehicleBillingTypeService, 
            IRouteContractorService routeContractorService, IMapper mapper, IRouteTimingService routeTimingService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(LogisticsController)) ?? throw new ArgumentNullException(nameof(logger));
            this.transporterService = transporterService ?? throw new ArgumentNullException(nameof(transporterService));
            this.routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            this.vehicleTypeService = vehicleTypeService ?? throw new ArgumentNullException(nameof(vehicleTypeService));
            this.vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            this.vendorService = vendorService ?? throw new ArgumentNullException(nameof(vendorService));
            this.vehicleBillingTypeService = vehicleBillingTypeService ?? throw new ArgumentNullException(nameof(vehicleBillingTypeService));
            this.routeContractorService = routeContractorService ?? throw new ArgumentNullException(nameof(routeContractorService));
            this.routeTimingService = routeTimingService ?? throw new ArgumentNullException(nameof(routeTimingService));
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
                return Ok(new { message = "Vehicle added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in AddVehicle", ex);
                return StatusCode(500, "An error occurred while adding the Vehicle");
            }
        }

        [HttpPut("update-vehicle")]
        public async Task<IActionResult> UpdateVehicle([FromBody] VehicleUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.VehicleId <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<VehicleUpdateRequest, VehicleUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                 { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                });

            await vehicleService.UpdateVehicle(requestParams);

            logger.LogInfo($"Vehicle Type with id {request.VehicleId} updated successfully.");
            return Ok(new { message = "Vehicle updated successfully." });
        }


        [HttpDelete("vehicle-delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await vehicleService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Vehicle with id {id} deleted successfully.");
                return Ok(new { message = "Vehicle deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Vehicle with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Vehicle");
            }
        }

        #endregion
        #region Vendor

        [HttpPost("list-vendor")]
        public async Task<IActionResult> GetVendorList([FromBody] ListsRequest request)
        {
            var result = await vendorService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("vendor/{vendorId}")]
        public async Task<ActionResult<VendorResponse?>> GetByVendorId(int vendorId)
        {
            try
            {
                logger.LogInfo($"Get Vendor by Id called: {vendorId}");
                var result = await vendorService.GetByVendorId(vendorId);
                if (result == null)
                {
                    logger.LogInfo($"Vendor with Id {vendorId} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Vendor with Id {vendorId} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Vendor with Id: {vendorId}", ex);
                return StatusCode(500, "An error occurred while retrieving the vendor.");
            }
        }

        [HttpPost("add-vendor")]
        public async Task<IActionResult> AddVendor([FromBody] VendorInsertRequestModel request)
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
                logger.LogInfo($"Add Vendor called: {request.VendorName}");

                var mappedRequest = mapper.MapWithOptions<VendorInsertRequest, VendorInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await vendorService.AddVendor(mappedRequest);
                logger.LogInfo($"Vendor {request.VendorName} added successfully.");
                return Ok(new { message = "Vendor added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding vendor", ex);
                return StatusCode(500, "An error occurred while adding the vendor.");
            }
        }

        [HttpPut("update-Id")]
        public async Task<IActionResult> UpdateVendor([FromBody] VendorUpdateRequestModel request)
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
                logger.LogInfo($"Update Vendor called: {request.VendorId}");

                var mappedRequest = mapper.MapWithOptions<VendorUpdateRequest, VendorUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await vendorService.UpdateVendor(mappedRequest);
                logger.LogInfo($"Vendor {request.VendorId} updated successfully.");
                return Ok(new { message = "Vendor updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating vendor", ex);
                return StatusCode(500, "An error occurred while updating the vendor.");
            }
        }

        [HttpDelete("vendor-delete/{vendorId}")]
        public async Task<IActionResult> DeleteVendor(int vendorId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await vendorService.DeleteVendor(vendorId, Convert.ToInt64(userId));
                logger.LogInfo($"Vendor with Id {vendorId} deleted successfully.");
                return Ok(new { message = "Vendor deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Vendor with Id: {vendorId}", ex);
                return StatusCode(500, "An error occurred while deleting the vendor.");
            }
        }

        #endregion
        #region Vehicle Billing Type
        [HttpPost("list-vehiclebillingtype")]
        public async Task<IActionResult> GetVehicleBillingTypeList([FromBody] ListsRequest request)
        {
            var result = await vehicleBillingTypeService.GetAllVehicleBillingTypes(request);
            return Ok(result);
        }

        [HttpGet("vehiclebillingtype/{id}")]
        public async Task<ActionResult<VehicleBillingTypeResponse?>> GetVehicleBillingTypeById(long id)
        {
            try
            {
                logger.LogInfo($"GetById called for Vehicle Billing Type Code: {id}");
                var result = await vehicleBillingTypeService.GetVehicleBillingTypeById(id);

                if (result == null)
                {
                    logger.LogInfo($"Vehicle Billing Type with Code {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Vehicle Billing Type with Code {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Vehicle Billing Type with Code: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }


        [HttpPost("Insert-vehiclebillingtype")]
        public async Task<IActionResult> AddVehicleBillingType([FromBody] VehicleBillingTypeInsertRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<VehicleBillingTypeInsertRequest, VehicleBillingTypeInsertRequestModel>(
                  request,
                  new Dictionary<string, object>
                  {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                  });
                await vehicleBillingTypeService.AddVehicleBillingType(mappedRequest);
                logger.LogInfo("Vehicle Billing Type added successfully.");
                return Ok(new { message = "Vehicle Billing Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding vehicle billing type", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }

        [HttpPut("update-vehiclebillingtype")]
        public async Task<IActionResult> UpdateVehicleBillingType([FromBody] VehicleBillingTypeUpdateRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<VehicleBillingTypeUpdateRequest, VehicleBillingTypeUpdateRequestModel>(
                  request,
                  new Dictionary<string, object>
                  {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                  });
                await vehicleBillingTypeService.UpdateVehicleBillingType(mappedRequest);
                logger.LogInfo($"Vehicle Billing Type with code {request.VehicleId} updated successfully.");
                return Ok(new { message = "Vehicle Billing Type updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating vehicle billing type with code ", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }


        [HttpDelete("delete-vehiclebillingtype/{id}")]
        public async Task<IActionResult> DeleteVehicleBillingType(long id)
        {
            try
            {
                var deletedBy = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value ?? "system";

                await vehicleBillingTypeService.DeleteVehicleBillingType(id, deletedBy);
                logger.LogInfo($"Vehicle Billing Type with Code {id} deleted successfully.");
                return Ok(new { message = "Vehicle Billing Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Vehicle Billing Type with Code: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the vehicle billing type. " + ex.Message);
            }
        }
        #endregion
        #region Route Contractor

        [HttpPost("contractor/list")]
        public async Task<IActionResult> GetContractorList([FromBody] ListsRequest request)
        {
            try
            {
                var result = await routeContractorService.GetAll(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving contractor list.", ex);
                return StatusCode(500, "An error occurred while retrieving the contractor list.");
            }
        }

        [HttpGet("contractor/{id}")]
        public async Task<IActionResult> GetContractorById(int id)
        {
            try
            {
                logger.LogInfo($"GetContractorById called for ID: {id}");
                var result = await routeContractorService.GetRouteContractorById(id);
                if (result == null)
                    return NotFound(new { message = $"Route Contractor with ID {id} not found." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving contractor with ID: {id}", ex);
                return StatusCode(500, $"An error occurred while retrieving contractor with ID {id}.");
            }
        }

        [HttpPost("contractor/add")]
        public async Task<IActionResult> AddContractor([FromBody] RouteContractorInsertRequestModel request)
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
                logger.LogInfo($"Add Contractor called: {request.ContractorName}");

                var mappedRequest = mapper.MapWithOptions<RouteContractorInsertRequest, RouteContractorInsertRequestModel>(
                    request,
                    new Dictionary<string, object>{
                            { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await routeContractorService.InsertRouteContractor(mappedRequest);
                logger.LogInfo($"Route Contractor '{request.ContractorName}' added successfully.");
                return Ok(new { message = "Route Contractor added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding Route Contractor", ex);
                return StatusCode(500, "An error occurred while adding the Route Contractor.");
            }
        }
        [HttpPut("contractor/update")]
        public async Task<IActionResult> UpdateContractor([FromBody] RouteContractorUpdateRequestModel request)
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
                logger.LogInfo($"Update Contractor called: {request.RouteContractorId}");

                var mappedRequest = mapper.MapWithOptions<RouteContractorUpdateRequest, RouteContractorUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await routeContractorService.UpdateRouteContractor(mappedRequest);
                logger.LogInfo($"Route Contractor with ID {mappedRequest.RouteContractorId} updated successfully.");
                return Ok(new { message = "Route Contractor updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating Route Contractor", ex);
                return StatusCode(500, "An error occurred while updating the Route Contractor.");
            }
        }

        [HttpDelete("contractor/delete/{id}")]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await routeContractorService.DeleteRouteContractor(id, Convert.ToInt32(userId));

                logger.LogInfo($"Route Contractor with ID {id} deleted successfully.");
                return Ok(new { message = "Route Contractor deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Route Contractor with ID: {id}", ex);
                return StatusCode(500, $"An error occurred while deleting the Route Contractor record.");
            }
        }

        #endregion
        
        #region Route-Timing
        [HttpPost]
        [Route("routeTiming-list")]
        public async Task<IActionResult> RouteTimingList([FromBody] ListsRequest request)
        {
            var result = await routeTimingService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("routeTimingID{id}")]
        public async Task<ActionResult<RouteTimingResponse?>> GetRouteTimingById(int id)
        {
            try
            {
                logger.LogInfo($"Get Route Timing by id called for id: {id}");
                var timing = await routeTimingService.GetById(id);
                if (timing == null)
                {
                    logger.LogInfo($"Route Timing with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Route Timing with id {id} retrieved successfully.");
                return Ok(timing);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Route Timing with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Route Timing.");
            }
        }

        [HttpPost]
        [Route("Insert-routeTiming")]
        public async Task<IActionResult> InsertRouteTiming([FromBody] RouteTimingInsertRequestModel request)
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
                logger.LogInfo($"Add called for Route Timing with RouteId: {request.RouteId}");
                var requestParams = mapper.MapWithOptions<RouteTimingInsertRequest, RouteTimingInsertRequestModel>(request,
                    new Dictionary<string, object> {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt32(UserId) }
                    });
                await routeTimingService.InsertRouteTiming(requestParams);
                logger.LogInfo($"Route Timing for RouteId {request.RouteId} added successfully.");
                return Ok(new { message = "Route Timing added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding Route Timing", ex);
                return StatusCode(500, "An error occurred while adding the Route Timing.");
            }
        }

        [HttpPut]
        [Route("update-routeTiming")]
        public async Task<IActionResult> UpdateRouteTiming([FromBody] RouteTimingUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.RouteTimingId <= 0)
                return BadRequest("Invalid request.");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<RouteTimingUpdateRequest, RouteTimingUpdateRequestModel>(request,
                new Dictionary<string, object> {
            { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(UserId) }
                });
            await routeTimingService.UpdateRouteTiming(requestParams);
            logger.LogInfo($"Route Timing with id {request.RouteTimingId} updated successfully.");
            return Ok(new { message = "Route Timing updated successfully." });
        }

        [HttpDelete("routeTiming-delete/{id}")]
        public async Task<IActionResult> DeleteRouteTiming(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await routeTimingService.DeleteRouteTiming(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Route Timing with id {id} deleted successfully.");
                return Ok(new { message = "Route Timing deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Route Timing with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Route Timing.");
            }
        }

        #endregion
    }
}
