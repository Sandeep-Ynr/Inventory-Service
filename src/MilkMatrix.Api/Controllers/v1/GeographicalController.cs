using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Geographical.District;
using MilkMatrix.Api.Models.Request.Geographical.Hamlet;
using MilkMatrix.Api.Models.Request.Geographical.State;
using MilkMatrix.Api.Models.Request.Geographical.Tehsil;
using MilkMatrix.Api.Models.Request.Geographical.Village;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request;

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

        public GeographicalController(IHttpContextAccessor httpContextAccessor, ILogging logging, IStateService stateService, IDistrictService districtService, ITehsilService tehsilService, IVillageService villageService, IHamletService hamletService)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            this.districtService = districtService ?? throw new ArgumentNullException(nameof(districtService));
            this.tehsilService = tehsilService ?? throw new ArgumentNullException(nameof(tehsilService));
            this.villageService = villageService ?? throw new ArgumentNullException(nameof(villageService));
            this.hamletService = hamletService?? throw new ArgumentNullException(nameof(hamletService));
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

            var response = request.ActionType == Domain.Entities.Enums.GetActionType.All
                ? await stateService.GetStates(stateRequest)
                : await stateService.GetSpecificLists(stateRequest);

            return response.Any() ? Ok(response) : BadRequest();
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
                ActionType = (Domain.Entities.Enums.GetActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == Domain.Entities.Enums.GetActionType.All
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
                StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (Domain.Entities.Enums.GetActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == Domain.Entities.Enums.GetActionType.All
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
                DistrictId = request.DistrictId,
                StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (Domain.Entities.Enums.GetActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == Domain.Entities.Enums.GetActionType.All
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
                TehsilId = request.TehsilId,
                DistrictId = request.DistrictId,
                StateId = request.StateId,
                //ActionType = request.ActionType,
                ActionType = (Domain.Entities.Enums.GetActionType)request.ActionType,
                IsActive = true
            };

            var response = request.ActionType == Domain.Entities.Enums.GetActionType.All
                ? await hamletService.GetHamlets(hamletRequest)
                : await hamletService.GetSpecificLists(hamletRequest);

            return response.Any() ? Ok(response) : BadRequest();
        }

        [HttpPost]
        [Route("add-state")]
        public async Task<IActionResult> AddState([FromBody] StateRequestModel request)
        {
            // 1.Validate incoming JSON
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var state = new StateRequest
            {
                ActionType = request.ActionType,
                StateId = request.StateId,
                StateName = request.StateName,
                AreaCode = request.AreaCode,
                CountryId = request.CountryId,
                CreatedBy = request.CreatedBy,
                ModifyBy = request.ModifyBy,
                IsActive = request.IsActive,
            };
            var created = await stateService.AddStateAsync(state);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add state.");

            // 5. Return 201 Created with the new resource
            return CreatedAtAction(
                nameof(GetStates),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = state.StateId
                },
                state
            );
        }


        [HttpPost]
        [Route("add-Districts")]
        public async Task<IActionResult> AddDistrictsAsync([FromBody] DistrictRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var district = new DistrictRequest
            {
                DistrictId = request.DistrictId,
                ModifyBy = request.ModifyBy
            };
            var created = await districtService.AddDistrictsAsync(district);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add state.");

            // 5. Return 201 Created with the new resource
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
            var Tehsil = new TehsilRequest
            {
                TehsilId = request.TehsilId,
                TehsilName = request.TehsilName,
                DistrictId = request.DistrictId,
                StateId = request.StateId,
                IsStatus = request.IsStatus,
                CreatedBy = request.CreatedBy,
                ModifyBy = request.ModifyBy,
            };
            var created = await tehsilService.AddTehsil(Tehsil);
            if (created == "Failed.")
                return StatusCode(500, "Failed to add state.");

            // 5. Return 201 Created with the new resource
            return CreatedAtAction(
                nameof(GetDistricts),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    controller = "Geographical",
                    id = Tehsil.TehsilId
                },
                Tehsil
            );
        }


    }
}
