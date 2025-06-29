using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Geographical.District;
using MilkMatrix.Api.Models.Request.Geographical.Hamlet;
using MilkMatrix.Api.Models.Request.Geographical.State;
using MilkMatrix.Api.Models.Request.Geographical.Tehsil;
using MilkMatrix.Api.Models.Request.Geographical.Village;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Geographical;
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

        public GeographicalController(IHttpContextAccessor httpContextAccessor, ILogging logging, IStateService stateService, IDistrictService districtService, ITehsilService tehsilService, IVillageService villageService, IHamletService hamletService, IMapper mapper)
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
            this.mapper = mapper;
        }

        /// <summary>
        /// Get the list of states.
        /// </summary>
        /// <returns>List of states.</returns>
        [HttpPost]
        [Route("state-list")]
        public async Task<IActionResult> GetStates([FromBody] StateRequestModel request)
        {

            logger.LogInfo($"GetStates request processed with ActionType: " +
                $"{request.ActionType}, StateId: " +
                $"{request.StateId}, CountryId:" +
                $" {request.CountryId}");

            var stateRequest = new StateRequest
            {
                StateId = request.StateId,
                CountryId = request.CountryId,
                ActionType = request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await stateService.GetStates(stateRequest)
                : await stateService.GetSpecificLists(stateRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }

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
        public async Task<IActionResult> GetDistricts([FromBody] DistrictRequestModel request)
        {
            logger.LogInfo($"GetDistricts request processed with ActionType: " +
                $"{request.ActionType}, DistrictId: " +
                $"{request.DistrictId}, StateId:" +
                $" {request.StateId}");

            var districtRequest = new DistrictRequest
            {
                DistrictId = request.DistrictId,
                StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await districtService.GetDistricts(districtRequest)
                : await districtService.GetSpecificLists(districtRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }

        /// <summary>
        /// Get the List of Tehsil
        /// </summary>
        /// <returns>List of Tehsil</returns>
        [HttpPost]
        [Route("tehsil-list")]
        public async Task<IActionResult> GetTehsils([FromBody] TehsilRequestModel request)
        {
            logger.LogInfo($"GetTehsils request processed with ActionType: " +
                $"{request.ActionType}, TehsilId: " +
                $"{request.TehsilId}, DistrictId: " +
                $"{request.DistrictId}, StateId:" +
                $" {request.StateId}");

            var tehsilRequest = new TehsilRequest
            {
                TehsilId = request.TehsilId,
                DistrictId = request.DistrictId,
                //StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await tehsilService.GetTehsils(tehsilRequest)
                : await tehsilService.GetSpecificLists(tehsilRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }

        /// <summary>
        /// Get the List of Village
        /// </summary>
        /// <returns>List of Village</returns>
        [HttpPost]
        [Route("village-list")]
        public async Task<IActionResult> GetVillages([FromBody] VillageRequestModel request)
        {
            logger.LogInfo($"GetVillages request processed with ActionType: " +
                $"{request.ActionType}, VillageId: " +
                $"{request.VillageId}, TehsilId: " +
                $"{request.TehsilId}, DistrictId: " +
                $"{request.DistrictId}, StateId:" +
                $" {request.StateId}");

            var villageRequest = new VillageRequest
            {
                VillageId = request.VillageId,
                TehsilId = request.TehsilId,
                //DistrictId = request.DistrictId,
                //StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await villageService.GetVillages(villageRequest)
                : await villageService.GetSpecificLists(villageRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Hamlet List</returns>
        [HttpPost]
        [Route("hamlet-list")]
        public async Task<IActionResult> GetHamlets([FromBody] HamletRequestModel request) 
        {
            logger.LogInfo($"GetHamlets request processed with ActionType: " +
                $"{request.ActionType}, HamletId: " +
                $"{request.HamletId}, VillageId: " +
                $"{request.VillageId}, TehsilId: " +
                $"{request.TehsilId}, DistrictId: " +
                $"{request.DistrictId}, StateId:" +
                $" {request.StateId}");

            var hamletRequest = new HamletRequest
            {
                HamletId = request.HamletId,
                VillageId = request.VillageId,
                //TehsilId = request.TehsilId,
                //DistrictId = request.DistrictId,
                //StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == ReadActionType.All
                ? await hamletService.GetHamlets(hamletRequest)
                : await hamletService.GetSpecificLists(hamletRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }


        [HttpPost]
        [Route("add-village")]
        public async Task<IActionResult> AddVillage([FromBody] VillageRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var village = mapper.Map<VillageRequest>(request);

            var created = await villageService.AddVillage(village);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add village.");

            return CreatedAtAction(
                nameof(GetVillages),  
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = village.VillageId
                },
                village
            );
        }
        [HttpPut]
        [Route("update-village/{id}")]
        public async Task<IActionResult> UpdateVillage(int id, [FromBody] VillageRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            // Ensure the route ID is used
            request.VillageId = id;

            var village = mapper.Map<VillageRequest>(request);

            var result = await villageService.UpdateVillage(village);
            if (result == "Failed.")
                return StatusCode(500, "Failed to update village.");

            return Ok(new { message = "Village updated successfully." });
        }

        [HttpDelete]
        [Route("delete-village/{id}")]
        public async Task<IActionResult> DeleteVillage(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid village ID.");

            var result = await villageService.DeleteVillage(id);

            if (result == "Failed." || result.Contains("failed", StringComparison.OrdinalIgnoreCase))
                return StatusCode(500, "Failed to delete village.");

            return Ok(new { message = "Village deleted successfully." });
        }


        [HttpPost]
        [Route("add-Districts")]
        public async Task<IActionResult> AddDistrictsAsync([FromBody] DistrictRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var district = mapper.Map<DistrictRequest>(request);

            var created = await districtService.AddDistrictsAsync(district);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add District.");

            return CreatedAtAction(
                nameof(GetDistricts),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = district.DistrictId
                },
                district
            );
        }


        [HttpPost]
        [Route("add-Tehsil")]
        public async Task<IActionResult> AddTehsil([FromBody] TehsilRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Tehsil = mapper.Map<TehsilRequest>(request);
            var created = await tehsilService.AddTehsil(Tehsil);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add Tehsil.");

            return CreatedAtAction(
                nameof(GetTehsils),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = Tehsil.TehsilId
                },
                Tehsil
            );
        }

        [HttpPost]
        [Route("add-Hamlet")]
        public async Task<IActionResult> AddHamlet([FromBody] HamletRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Hamlet = mapper.Map<HamletRequest>(request);
            var created = await hamletService.AddHamlet(Hamlet);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add Hamlet.");

            return CreatedAtAction(
                nameof(GetHamlets),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = Hamlet.HamletId
                },
                Hamlet
            );
        }
    }
}
