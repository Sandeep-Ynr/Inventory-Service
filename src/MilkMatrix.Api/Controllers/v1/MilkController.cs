using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Milk;
using MilkMatrix.Api.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Api.Models.Request.Milk.DockData;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Milk;
using MilkMatrix.Milk.Contracts.Milk.DeviceSetting;
using MilkMatrix.Milk.Contracts.Milk.DockData;
using MilkMatrix.Milk.Contracts.Milk.MilkCollection;
using MilkMatrix.Milk.Implementations.Milk.DeviceSetting;
using MilkMatrix.Milk.Implementations.Milk.DockData;


//using MilkMatrix.Milk.Implementations.Milk;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Milk;
using MilkMatrix.Milk.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Request.Milk.DockData;
using MilkMatrix.Milk.Models.Response.Milk;
using MilkMatrix.Milk.Models.Response.Milk.DeviceSetting;
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
        private readonly IMilkCollectionService milkcollectionservice;
        private readonly IDeviceSettingService deviceSettingService;
        private readonly IDockDataService dockDataService;

        public MilkController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IMilkService milkService, IDeviceSettingService deviceSettingService, IMilkCollectionService milkcollectionservice, IDockDataService dockDataService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.deviceSettingService = deviceSettingService ?? throw new ArgumentNullException(nameof(deviceSettingService));
            this.milkcollectionservice = milkcollectionservice ?? throw new ArgumentNullException(nameof(milkcollectionservice));
            this.mapper = mapper;
            this.milkService = milkService;
            this.dockDataService = dockDataService ?? throw new ArgumentNullException(nameof(dockDataService));
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

                logger.LogInfo($"Add called for Measurement Unit: {request.MilkTypeName}");
                var requestParams = mapper.MapWithOptions<MilkTypeInsertRequest, MilkTypeInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await milkService.AddAsync(requestParams);
                logger.LogInfo($"Measurement Unit {request.MilkTypeName} added successfully.");
                return Ok(new { message = "Milk Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Measurement Unit", ex);
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

        [HttpPost]
        [Route("rateypelist")]
        public async Task<IActionResult> RateList([FromBody] ListsRequest request)
        {
            var result = await milkService.GetAllRateTypeAsync(request);
            return Ok(result);
        }

        [HttpGet("ratetype{id}")]
        public async Task<ActionResult<RateTypeInsertResponse?>> GetRatetypeById(int id)
        {
            try
            {
                logger.LogInfo($"Get Milk Type by id called for id: {id}");
                var mcc = await milkService.GetRateTypeByIdAsync(id);
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
        [Route("addratetype")]
        public async Task<IActionResult> AddRateAsync([FromBody] RateTypeInsertRequestModel request)
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

                logger.LogInfo($"Add called for Rate Type: {request.RateTypeName}");
                var requestParams = mapper.MapWithOptions<RateTypeInsertRequest, RateTypeInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await milkService.AddRateTypeAsync(requestParams);
                logger.LogInfo($"Rate Type {request.RateTypeName} added successfully.");
                return Ok(new { message = "Rate Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Rate Type", ex);
                return StatusCode(500, "An error occurred while adding the Rate Type.");
            }
        }

        [HttpPut]
        [Route("updateratetype/{id}")]
        public async Task<IActionResult> UpdateRateTypAsync(int id, [FromBody] RateTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<RateTypeUpdateRequest, RateTypeUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await milkService.UpdateRateTypeAsync(requestParams);
            logger.LogInfo($"Rate Type with id {request.RateTypeId} updated successfully.");
            return Ok(new { message = "Rate Type updated successfully." });
        }

        [HttpDelete("deleteratetype/{id}")]
        public async Task<IActionResult> DeleteRateType(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await milkService.DeleteRateTypeAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Rate Type with id {id} deleted successfully.");
                return Ok(new { message = "Rate Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Rate Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Rate Type.");
            }
        }


        [HttpPost]
        [Route("measurement-unit-list")]
        public async Task<IActionResult> MeasurementUnitList([FromBody] ListsRequest request)
        {
            var result = await milkService.GetAllMeasureUnitAsync(request);
            return Ok(result);
        }

        [HttpGet("measurement-unit{id}")]
        public async Task<ActionResult<MeasurementUnitInsertResponse?>> GetMeasurementUnitById(int id)
        {
            try
            {
                logger.LogInfo($"Get Measurement Unit by id called for id: {id}");
                var mcc = await milkService.GetMeasureUnitByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Measurement Unit with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Measurement Unit with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Measurement Unit with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Measurement Unit.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync([FromBody] MeasurementUnitInsertModel request)
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

                logger.LogInfo($"Add called for Measurement Unit: {request.MeasurementUnitName}");
                var requestParams = mapper.MapWithOptions<MeasurementUnitInsertRequest, MeasurementUnitInsertModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await milkService.AddAsync(requestParams);
                logger.LogInfo($"Measurement Unit {request.MeasurementUnitName} added successfully.");
                return Ok(new { message = "Measurement Unit added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Measurement Unit", ex);
                return StatusCode(500, "An error occurred while adding the Measurement Unit.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MeasurementUnitUpdateModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<MeasurementUnitUpdateRequest, MeasurementUnitUpdateModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await milkService.UpdateAsync(requestParams);
            logger.LogInfo($"Measurement Unit with id {request.MeasurementUnitId} updated successfully.");
            return Ok(new { message = "Measurement Unit updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMeasureUnitAsync(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await milkService.DeleteMeasureUnitAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Measurement Unit with id {id} deleted successfully.");
                return Ok(new { message = "Measurement Unit deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Measurement Unit with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Measurement Unit.");
            }
        }

        #region Device Settings
        [HttpPost("DeviceSetting-list")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            try
            {
                var result = await deviceSettingService.GetAll(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving Device Settings list", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the list.",

                });
            }
        }

        [HttpGet("DeviceSetting{id}")]
        public async Task<ActionResult<DeviceSettingResponse?>> GetDeviceSettingById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for DeviceSetting ID: {id}");
                var result = await deviceSettingService.GetDeviceSettingById(id);
                if (result == null)
                {
                    logger.LogInfo($"Device Setting with ID {id} not found.");
                    return NotFound(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        ErrorMessage = "Device Setting not found."
                    });
                }

                logger.LogInfo($"Device Setting with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving DeviceSetting with ID: {id}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the record.",
                });
            }
        }
        [HttpPost]
        [Route("Insert-DeviceSetting")]
        public async Task<IActionResult> InsertDeviceSetting([FromBody] DeviceSettingInsertRequestModel request)
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
                var requestParams = mapper.MapWithOptions<DeviceSettingInsertRequest, DeviceSettingInsertRequestModel>(request
                    , new Dictionary<string, object> {
                          { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await deviceSettingService.InsertDeviceSetting(requestParams);
                logger.LogInfo($"Device Setting for MPP ID {request.MppId} added successfully.");
                return Ok(new { message = "Device Setting added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding DeviceSetting", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while adding the record.",
                });
            }
        }


        [HttpPut]
        [Route("Update-DeviceSetting")]
        public async Task<IActionResult> UpdateBankType([FromBody] DeviceSettingUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.DeviceSettingId <= 0)
                    return BadRequest("Invalid request.");
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<DeviceSettingUpdateRequest, DeviceSettingUpdateRequestModel>(request
                            , new Dictionary<string, object> {
                      {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                        });
                await deviceSettingService.UpdateDeviceSetting(requestParams);
                logger.LogInfo($"Device Setting {request.DeviceSettingId} updated successfully.");
                return Ok(new { message = "Device Setting updated successfully." });
            }
            catch (Exception ex)
            {

                logger.LogError($"Error updating DeviceSetting with ID {request?.DeviceSettingId}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while updating the record.",
                });
            }

        }


        [HttpDelete("Delete-DeviceSetting/{id}")]
        public async Task<IActionResult> DeleteDeviceSetting(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await deviceSettingService.DeleteDeviceSetting(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Device Setting with id {id} deleted successfully.");
                return Ok(new { message = "Device Setting deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting DeviceSetting with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Bank Type.");
            }
        }
        #endregion

        #region MilkCollection

        [HttpPost("MilkCollection-list")]
        public async Task<IActionResult> GetMilkCollectionList([FromBody] ListsRequest request)
        {
            try
            {
                var result = await milkcollectionservice.GetMilkCollectionAll(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving Milk Collection list", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the list.",
                });
            }
        }

        [HttpGet("MilkCollection/{id}")]
        public async Task<ActionResult<MilkCollectionResponse?>> GetMilkCollectionById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for MilkCollection ID: {id}");
                var result = await milkcollectionservice.GetMilkCollectionById(id);
                if (result == null)
                {
                    logger.LogInfo($"Milk Collection with ID {id} not found.");
                    return NotFound(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        ErrorMessage = "Milk Collection record not found."
                    });
                }

                logger.LogInfo($"Milk Collection with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MilkCollection with ID: {id}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the record.",
                });
            }
        }

        [HttpPost("Insert-MilkCollection")]
        public async Task<IActionResult> InsertMilkCollection([FromBody] MilkCollectionInsertRequestModel request)
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

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<MilkCollectionInsertRequest, MilkCollectionInsertRequestModel>(request,
                    new Dictionary<string, object> {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await milkcollectionservice.InsertMilkCollection(requestParams);
                logger.LogInfo($"Milk Collection record added successfully.");
                return Ok(new { message = "Milk Collection record added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding Milk Collection", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while adding the record.",
                });
            }
        }

        [HttpPut("Update-MilkCollection")]
        public async Task<IActionResult> UpdateMilkCollection([FromBody] MilkCollectionUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.CollectionId <= 0)
                    return BadRequest("Invalid request.");

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<MilkCollectionUpdateRequest, MilkCollectionUpdateRequestModel>(request,
                    new Dictionary<string, object> {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                    });

                await milkcollectionservice.UpdateMilkCollection(requestParams);
                logger.LogInfo($"Milk Collection {request.CollectionId} updated successfully.");
                return Ok(new { message = "Milk Collection updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating MilkCollection with ID {request?.CollectionId}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while updating the record.",
                });
            }
        }

        [HttpDelete("Delete-MilkCollection/{id}")]
        public async Task<IActionResult> DeleteMilkCollection(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await milkcollectionservice.DeleteMilkCollection(id, Convert.ToInt32(userId));
                logger.LogInfo($"Milk Collection with id {id} deleted successfully.");
                return Ok(new { message = "Milk Collection deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting MilkCollection with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Milk Collection record.");
            }
        }
        #endregion
    

        #region DockData

        [HttpPost("DockData-list")]
        public async Task<IActionResult> GetDockDataList([FromBody] ListsRequest request)
        {
            try
            {
                var result = await dockDataService.GetAll(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving DockData list", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the list.",
                });
            }
        }

        [HttpGet("DockData/{id}")]
        public async Task<IActionResult> GetDockDataById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for DockData ID: {id}");

                var result = await dockDataService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"DockData with ID {id} not found.");
                    return NotFound(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        ErrorMessage = "DockData record not found."
                    });
                }

                logger.LogInfo($"DockData with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving DockData with ID: {id}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while retrieving the record.",
                });
            }
        }


        [HttpPost("Insert-DockData")]
        public async Task<IActionResult> InsertDockData([FromBody] DockDataInsertRequestModel request)
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

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<DockDataInsertRequest, DockDataInsertRequestModel>(request,
                    new Dictionary<string, object> {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await dockDataService.InsertDockData(requestParams);
                logger.LogInfo($"DockData record added successfully.");
                return Ok(new { message = "DockData record added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding DockData", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "An error occurred while adding the record.",
                });
            }
        }


        [HttpPut("Dockdata-update")]
        public async Task<IActionResult> Update([FromBody] DockDataUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.DockDataUpdateId <= 0)
                {
                    return BadRequest("Invalid request.");
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<DockDataUpdateRequest, DockDataUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                 { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await dockDataService.UpdateDockData(mappedRequest);

                logger.LogInfo($"DockData with ID {request.DockDataUpdateId} updated successfully.");
                return Ok(new { message = "DockData updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Update DockData", ex);
                return StatusCode(500, $"An error occurred while updating the record. {ex.Message}");
            }
        }

        [HttpDelete("Dockdata-delete/{id}")]
        public async Task<IActionResult> DockDataDelete(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await dockDataService.DockDataDelete(id, Convert.ToInt32(userId));
                logger.LogInfo($"DockData with ID {id} deleted successfully.");
                return Ok(new { message = "DockData deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting DockData with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the DockData record.");
            }
        }

        #endregion

    }
}
