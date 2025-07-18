using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Animal;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Animal;
using MilkMatrix.Milk.Implementations.Animal;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Animal;
using MilkMatrix.Milk.Models.Response.Animal;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AnimalController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IAnimalService animalService;

        public AnimalController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IAnimalService animalService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.animalService = animalService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await animalService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("AnimalType{id}")]
        public async Task<ActionResult<AnimalTypeInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get BMC by id called for id: {id}");
                var mcc = await animalService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"BMC with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"BMC with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving BMC with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the BMC.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync([FromBody] AnimalTypeInsertRequestModel request)
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

                logger.LogInfo($"Add called for Animal Type: {request.AnimalTypeName}");
                var requestParams = mapper.MapWithOptions<AnimalTypeInsertRequest, AnimalTypeInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await animalService.AddAsync(requestParams);
                logger.LogInfo($"Animal Type {request.AnimalTypeName} added successfully.");
                return Ok(new { message = "Animal Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Animal Type", ex);
                return StatusCode(500, "An error occurred while adding the Animal Type.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AnimalTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<AnimalTypeUpdateRequest, AnimalTypeUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await animalService.UpdateAsync(requestParams);
            logger.LogInfo($"Animal Type with id {request.AnimalTypeId} updated successfully.");
            return Ok(new { message = "Animal Type updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await animalService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Animal Type with id {id} deleted successfully.");
                return Ok(new { message = "Animal Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Animal Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Animal Type.");
            }
        }

    }
}
