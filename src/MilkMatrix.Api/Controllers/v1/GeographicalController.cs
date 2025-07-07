using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Api.Models.Request.Geographical.BankRegional;
using MilkMatrix.Api.Models.Request.Geographical.District;
using MilkMatrix.Api.Models.Request.Geographical.Hamlet;
using MilkMatrix.Api.Models.Request.Geographical.State;
using MilkMatrix.Api.Models.Request.Geographical.Tehsil;
using MilkMatrix.Api.Models.Request.Geographical.Village;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GeographicalController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly ILogging logger;

        private readonly IStateService stateService;

        private readonly IDistrictService districtService;

        private readonly ITehsilService tehsilService;

        private readonly IVillageService villageService;

        private readonly IHamletService hamletService;

        private readonly IMapper mapper;

        private readonly IBankRegService bankRegService;

        public GeographicalController(IHttpContextAccessor httpContextAccessor, ILogging logging, IStateService stateService, 
            IDistrictService districtService, ITehsilService tehsilService, IVillageService villageService, 
            IHamletService hamletService, IBankRegService bankRegService, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            //this.stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            this.stateService = stateService;
            this.districtService = districtService ?? throw new ArgumentNullException(nameof(districtService));
            this.tehsilService = tehsilService ?? throw new ArgumentNullException(nameof(tehsilService));
            this.villageService = villageService ?? throw new ArgumentNullException(nameof(villageService));
            this.hamletService = hamletService ?? throw new ArgumentNullException(nameof(hamletService));
            this.bankRegService = bankRegService ?? throw new ArgumentNullException(nameof(bankRegService));
            this.mapper = mapper;
        }

        /// <summary>
        /// Get the list of states.
        /// </summary>
        /// <returns>List of states.</returns>
        [HttpPost]
        [Route("state-list")]
        public async Task<IActionResult> StateList([FromBody] ListsRequest request)
        {
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var result = await stateService.GetAllAsync(request, Convert.ToInt32(UserId));
            return Ok(result);
        }

        //public async Task<IActionResult> GetStates([FromBody] StateRequestModel request)
        //{

        //    logger.LogInfo($"GetStates request processed with ActionType: " +
        //        $"{request.ActionType}, StateId: " +
        //        $"{request.StateId}, CountryId:" +
        //        $" {request.CountryId}");

        //    var stateRequest = new StateRequest
        //    {
        //        StateId = request.StateId,
        //        CountryId = request.CountryId,
        //        ActionType = request.ActionType,
        //        IsActive = true
        //    };

        //    var response = request.ActionType == ReadActionType.All
        //        ? await stateService.GetStates(stateRequest)
        //        : await stateService.GetSpecificLists(stateRequest);

        //    return response.Any() ? Ok(response) : BadRequest();
        //}

        [HttpPost("state-upsert")]
        public async Task<IActionResult> UpsertState([FromBody] StateUpsertModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                    });
                }
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                // If Id is present and > 0, treat as update, else add
                if (request.StateId != null && request.StateId > 0)
                {
                    logger.LogInfo($"Upsert: Update called for State id: {request.StateId}");
                    var requestParams = mapper.MapWithOptions<StateUpdateRequest, StateUpsertModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
                    await stateService.UpdateStateAsync(requestParams);
                    logger.LogInfo($"State with id {request.StateId} updated successfully.");
                    return Ok(new { message = "State updated successfully." });
                }
                else
                {
                    logger.LogInfo($"Upsert: Add called for State: {request.StateName}");
                    var requestParams = mapper.MapWithOptions<StateInsertRequest, StateUpsertModel>(request
                        , new Dictionary<string, object> {
                            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                    });
                    await stateService.AddStateAsync(requestParams);
                    logger.LogInfo($"State {request.StateName} added successfully.");
                    return Ok(new { message = "State added successfully." });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert State", ex);
                return StatusCode(500, "An error occurred while adding the State.");
            }
        }

        /// <summary>
        /// Delete State
        /// </summary>
        /// <returns>Delete State</returns>
        [HttpDelete("state-delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                logger.LogInfo($"Delete role called for id: {id}");
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await stateService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"role with id {id} deleted successfully.");
                return Ok(new { message = "State deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting role with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the role.");
            }
        }


        /// <summary>
        /// Get the List of District
        /// </summary>
        /// <returns>List of Districts</returns>
        [HttpPost]
        [Route("district-list")]
        public async Task<IActionResult> DistrictList([FromBody] ListsRequest request)
        {
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var result = await districtService.GetAllAsync(request, Convert.ToInt32(UserId));
            return Ok(result);
        }

        //public async Task<IActionResult> GetDistricts([FromBody] DistrictRequestModel request)
        //{
        //    logger.LogInfo($"GetDistricts request processed with ActionType: " +
        //        $"{request.ActionType}, DistrictId: " +
        //        $"{request.DistrictId}, StateId:" +
        //        $" {request.StateId}");

        //    var districtRequest = new DistrictRequest
        //    {
        //        DistrictId = request.DistrictId,
        //        StateId = request.StateId,
        //        //ActionType = request.ActionType,
        //        ActionType = (ReadActionType)request.ActionType,
        //        IsActive = true
        //    };

        //    var response = request.ActionType == ReadActionType.All
        //        ? await districtService.GetDistricts(districtRequest)
        //        : await districtService.GetSpecificLists(districtRequest);

        //    return response.Any() ? Ok(response) : BadRequest();
        //}

        [HttpGet("district{id}")]
        public async Task<ActionResult<DistrictResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get district by id called for id: {id}");
                var user = await districtService.GetByIdAsync(id);
                if (user == null)
                {
                    logger.LogInfo($"district with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"district with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving district with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the district.");
            }
        }

        [HttpPost]
        [Route("add-Districts")]
        public async Task<IActionResult> AddDistrictsAsync([FromBody] DistrictInsertRequestModel request)
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

                logger.LogInfo($"Add called for Tehsil: {request.DistrictName}");
                var requestParams = mapper.MapWithOptions<DistrictInsertRequest, DistrictInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await districtService.AddDistrictsAsync(requestParams);
                logger.LogInfo($"District {request.DistrictName} added successfully.");
                return Ok(new { message = "District added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Tehsil", ex);
                return StatusCode(500, "An error occurred while adding the Tehsil.");
            }
        }

        [HttpPut]
        [Route("update-district/{id}")]
        public async Task<IActionResult> UpdateDistrict(int id,[FromBody] DistrictUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            // Ensure the route ID is used
            //request.VillageId = id;
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<DistrictUpdateRequest, DistrictUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await districtService.UpdateDistrictAsync(requestParams);
            logger.LogInfo($"District with id {request.DistrictId} updated successfully.");
            return Ok(new { message = "District updated successfully." });
        }

        [HttpDelete("district-delete/{id}")]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await districtService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"District with id {id} deleted successfully.");
                return Ok(new { message = "District deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting role with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the District.");
            }
        }


        /// <summary>
        /// Get the List of Tehsil
        /// </summary>
        /// <returns>List of Tehsil</returns>
        [HttpPost]
        [Route("tehsil-list")]
        public async Task<IActionResult> TehsilList([FromBody] ListsRequest request)
        {
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var result = await tehsilService.GetAllAsync(request, Convert.ToInt32(UserId));
            return Ok(result);
        }
        //public async Task<IActionResult> GetTehsils([FromBody] TehsilRequestModel request)
        //{
        //    logger.LogInfo($"GetTehsils request processed with ActionType: " +
        //        $"{request.ActionType}, TehsilId: " +
        //        $"{request.TehsilId}, DistrictId: " +
        //        $"{request.DistrictId}");

        //    var tehsilRequest = new TehsilRequest
        //    {
        //        TehsilId = request.TehsilId,
        //        DistrictId = request.DistrictId,
        //        //StateId = request.StateId,
        //        //ActionType = request.ActionType,
        //        ActionType = (ReadActionType)request.ActionType,
        //        IsActive = true
        //    };

        //    var response = request.ActionType == ReadActionType.All
        //        ? await tehsilService.GetTehsils(tehsilRequest)
        //        : await tehsilService.GetSpecificLists(tehsilRequest);

        //    return response.Any() ? Ok(response) : BadRequest();
        //}

        /// <summary>
        /// Get Tehsil By Tehsil ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Tehsil Detail</returns>
        [HttpGet("tehsil{id}")]
        public async Task<ActionResult<TehsilResponse?>> GetTehsilById(int id)
        {
            try
            {
                logger.LogInfo($"Get tehsil by id called for id: {id}");
                var user = await tehsilService.GetByIdAsync(id);
                if (user == null)
                {
                    logger.LogInfo($"tehsil with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"tehsil with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving tehsil with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the tehsil.");
            }
        }

        [HttpPost]
        [Route("add-Tehsil")]
        public async Task<IActionResult> AddTehsil([FromBody] TehsilInsertRequestModel request)
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

                logger.LogInfo($"Add called for Tehsil: {request.TehsilName}");
                var requestParams = mapper.MapWithOptions<TehsilInsertRequest, TehsilInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await tehsilService.AddTehsilAsync(requestParams);
                logger.LogInfo($"Tehsil {request.TehsilName} added successfully.");
                return Ok(new { message = "Tehsil added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Tehsil", ex);
                return StatusCode(500, "An error occurred while adding the Tehsil.");
            }
        }
        

        [HttpPut]
        [Route("update-tehsil/{id}")]
        public async Task<IActionResult> UpdateTehsil(int id, [FromBody] TehsilUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            // Ensure the route ID is used
            //request.VillageId = id;
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<TehsilUpdateRequest, TehsilUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await tehsilService.UpdateTehsilAsync(requestParams);
            logger.LogInfo($"Tehsil with id {request.TehsilId} updated successfully.");
            return Ok(new { message = "Tehsil updated successfully." });
        
        }

        [HttpDelete("tehsil-delete/{id}")]
        public async Task<IActionResult> DeleteTehsil(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await tehsilService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Tehsil with id {id} deleted successfully.");
                return Ok(new { message = "Tehsil deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Tehsil with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Tehsil.");
            }
        }

        /// <summary>
        /// Get the List of Village
        /// </summary>
        /// <returns>List of Village</returns>
        [HttpPost]
        [Route("village-list")]
        public async Task<IActionResult> GetVillages([FromBody] VillageRequestModel request)
        {
            logger.LogInfo($"GetDistricts request processed with ActionType: " +
               $"{request.ActionType}, ActionType: " +
               $"{request.VillageId}, VillageId:" +
               $" {request.TehsilId}");

            var villageRequest = new VillageRequest
            {
                VillageId = request.VillageId,
                TehsilId = request.VillageId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await villageService.GetVillages(villageRequest)
                : await villageService.GetSpecificLists(villageRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }
        [HttpGet("village{id}")]
        public async Task<ActionResult<VillageResponse?>> GetByVillageId(int id)
        {
            try
            {
                logger.LogInfo($"Get village by id called for id: {id}");
                var user = await villageService.GetByVillageId(id);
                if (user == null )
                {
                    logger.LogInfo($"village with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"village with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving village with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the village.");
            }
        }
        [HttpPost]
        [Route("add-village")]
        public async Task<IActionResult> AddVillage([FromBody] VillageInsertRequestModel request)
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
                logger.LogInfo($"Add called for Village: {request.VillageName}");
                var requestParams = mapper.MapWithOptions<VillageInsertRequest, VillageInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await villageService.AddVillage(requestParams);
                logger.LogInfo($"Village {request.VillageName} added successfully.");
                return Ok(new { message = "Village added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Village", ex);
                return StatusCode(500, "An error occurred while adding the Village.");
            }
        }
        [HttpPut]
        [Route("update-Village")]
        public async Task<IActionResult> UpdateVillage([FromBody] VillageUpdateRequestModel request)
        {
            if (!ModelState.IsValid )
                return BadRequest("Invalid request.");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<VillageUpdateRequest, VillageUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await villageService.UpdateVillage(requestParams);
            logger.LogInfo($"Village with id {request.VillageId} updated successfully.");
            return Ok(new { message = "Village updated successfully." });
        }

        [HttpDelete("village-delete/{id}")]
        public async Task<IActionResult> DeleteVillage(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await villageService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Village with id {id} deleted successfully.");
                return Ok(new { message = "Village deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Village with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Village.");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Hamlet List</returns>
        [HttpPost]
        [Route("hamlet-list")]
        public async Task<ActionResult> GetHamlets([FromBody] HamletRequestModel request)
        {
            logger.LogInfo($"GetHamlets request processed with ActionType: " +
               $"{request.ActionType}, ActionType: " +
               $"{request.VillageId}, VillageId:" +
               $" {request.TehsilId}");
            var hamletRequest = new HamletRequest
            {
                VillageId = request.VillageId,
                HamletId = request.HamletId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };
            var response = request.ActionType == ReadActionType.All
                ? await hamletService.GetHamlets(hamletRequest)
                : await hamletService.GetSpecificLists(hamletRequest);
            return response.Any() ? Ok(response) : BadRequest();

        }
        
        [HttpGet("HamletId{id}")]
        public async Task<ActionResult<TehsilResponse?>> GetByHamletId(int id)
        {
            try
            {
                logger.LogInfo($"Get Hamlet by id called for id: {id}");
                var user = await hamletService.GetByHamletId(id);
                if (user == null)
                {
                    logger.LogInfo($"Hamlet with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Hamlet with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving village with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the village.");
            }
        }
        
        [HttpPost]
        [Route("add-hamlet")]
        public async Task<IActionResult> AddHamlet([FromBody] HamletInsertRequestModel request)
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
                logger.LogInfo($"Add called for Hamlet: {request.HamletName}");
                var requestParams = mapper.MapWithOptions<HamletInsertRequest, HamletInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await hamletService.AddHamlet(requestParams);
                logger.LogInfo($"Hamlet {request.HamletName} added successfully.");
                return Ok(new { message = "Hamlet added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Hamlet", ex);
                return StatusCode(500, "An error occurred while adding the Hamlet.");
            }
        }

        [HttpPut]
        [Route("update-hamlet")]
        public async Task<IActionResult> UpdateHamlet([FromBody] HamletUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.HamletId <= 0)
                return BadRequest("Invalid request.");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<HamletUpdateRequest, HamletUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await hamletService.UpdateHamlet(requestParams);
            logger.LogInfo($"Hamlet with id {request.VillageId} updated successfully.");
            return Ok(new { message = "Hamlet updated successfully." });
        }
        [HttpDelete("hamlet-delete/{id}")]
        
        public async Task<IActionResult> DeleteHamlet(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await hamletService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Hamlet with id {id} deleted successfully.");
                return Ok(new { message = "Hamlet deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Hamlet with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Hamlet.");
            }
        }

        [HttpPost]
        [Route("add-BankRegional")]
        public async Task<IActionResult> AddBankRegional([FromBody] BankRegInsertReqModel request)
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
                logger.LogInfo($"Add called for Bank Regional: {request.RegionalBankName}");
                var requestParams = mapper.MapWithOptions<BankRegInsertRequest, BankRegInsertReqModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await bankRegService.AddBankReg(requestParams);
                logger.LogInfo($"Hamlet {request.RegionalBankName} added successfully.");
                return Ok(new { message = "Bank Regional added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Bank Regional", ex);
                return StatusCode(500, "An error occurred while adding the RegionalBank.");
            }
        }


        [HttpPut]
        [Route("update-BankRegional")]
        public async Task<IActionResult> UpdateBankRegional([FromBody] BankRegUpdateReqModel request)
        {
            if (!ModelState.IsValid || request.RegionalID <= 0)
                return BadRequest("Invalid request.");
            var UserId = 3;// httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            logger.LogInfo($"Update called for Bank Regional ID: {request.RegionalID}");
            var requestParams = mapper.MapWithOptions<BankRegUpdateRequest, BankRegUpdateReqModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await bankRegService.UpdateBankReg(requestParams);
            logger.LogInfo($"Bank Regional with ID {request.RegionalCode} updated successfully.");
            return Ok(new { message = "Bank Regional updated successfully." });
        }



        [HttpDelete("delete-BankRegional/{id}")]
        public async Task<IActionResult> DeleteBankRegional(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await bankRegService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Bank Regional with id {id} deleted successfully.");
                return Ok(new { message = "Bank Regional deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Bank Regional with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Hamlet.");
            }
        }

        [HttpGet("bankregional/{id}")]
        public async Task<ActionResult<BankRegResponse>> GetBankRegionalById(int id)
        {
            try
            {
                logger.LogInfo($"Get Bank Regional by ID called: {id}");

                var regionalBank = await bankRegService.GetById(id);

                if (regionalBank == null)
                {
                    logger.LogInfo($"Bank Regional with ID {id} not found.");
                    return NotFound(new { message = $"Bank Regional with ID {id} not found." });
                }

                logger.LogInfo($"Bank Regional with ID {id} retrieved successfully.");
                return Ok(regionalBank);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Bank Regional with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Bank Regional.");
            }
        }

        [HttpPost]
        [Route("regional-bank-list")]
        public async Task<IActionResult> GetRegionalBank([FromBody] BankRegionalModel request)
        {
            logger.LogInfo($"GetRegionalBanks request processed with ActionType: {request.ActionType}, " +
                           $"RegionalId: {request.RegionalID}");

            var regionalRequest = new BankRegionalRequest
            {
                BankRegionalId = request.RegionalID,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = regionalRequest.ActionType == ReadActionType.All
                ? await bankRegService.GetBankReg(regionalRequest)
                : await bankRegService.GetSpecificLists(regionalRequest);

            return response != null && response.Any()
                ? Ok(response)
                : BadRequest("No records found.");
        }


    }
}
