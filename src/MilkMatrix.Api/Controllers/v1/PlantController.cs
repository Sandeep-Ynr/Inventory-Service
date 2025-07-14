using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Plant;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Plant;
using MilkMatrix.Milk.Implementations;

using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Plant;
using MilkMatrix.Milk.Models.Response.Plant;
using static MilkMatrix.Api.Common.Constants.Constants;


namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class PlantController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IPlantService plantService;

        public PlantController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IPlantService plantService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.plantService = plantService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> PlantList([FromBody] ListsRequest request)
        {
            var result = await plantService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("plant{id}")]
        public async Task<ActionResult<PlantInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Plant by id called for id: {id}");
                var plant = await plantService.GetByIdAsync(id);
                if (plant == null)
                {
                    logger.LogInfo($"Plant with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Plant with id {id} retrieved successfully.");
                return Ok(plant);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Plant with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Plant.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddPlantAsync([FromBody] PlantInsertRequestModel request)
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

                logger.LogInfo($"Add called for Plant: {request.PlantName}");
                var requestParams = mapper.MapWithOptions<PlantInsertRequest, PlantInsertRequestModel>(request
                    , new Dictionary<string, object> 
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await plantService.AddPlantAsync(requestParams);
                logger.LogInfo($"Plant {request.PlantName} added successfully.");
                return Ok(new { message = "Plant added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Plant", ex);
                return StatusCode(500, "An error occurred while adding the Plant.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdatePlantAsync(int id, [FromBody] PlantUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            // Ensure the route ID is used
            //request.VillageId = id;
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<PlantUpdateRequest, PlantUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await plantService.UpdatePlantAsync(requestParams);
            logger.LogInfo($"Plant with id {request.DistrictId} updated successfully.");
            return Ok(new { message = "Plant updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await plantService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Plant with id {id} deleted successfully.");
                return Ok(new { message = "Plant deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Plant with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Plant.");
            }
        }


    }
}
