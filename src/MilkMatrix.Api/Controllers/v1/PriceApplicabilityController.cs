using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.PriceApplicability;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.PriceApplicability;
using MilkMatrix.Milk.Implementations.Bmc;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.PriceApplicability;
using MilkMatrix.Milk.Models.Response.PriceApplicability;
using static MilkMatrix.Api.Common.Constants.Constants;


namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PriceApplicabilityController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IPriceApplicabilityService priceApplicabilityService;

        public PriceApplicabilityController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IPriceApplicabilityService priceApplicabilityService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.priceApplicabilityService = priceApplicabilityService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await priceApplicabilityService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PriceAppInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Price Applicability by id called for id: {id}");
                var mcc = await priceApplicabilityService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Price Applicability with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Price Applicability with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Price Applicability with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Price Applicability.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync([FromBody] PriceAppInsertRequestModel request)
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

                logger.LogInfo($"Add called for Price Applicability: {request.ModuleName}");
                var requestParams = mapper.MapWithOptions<PriceAppInsertRequest, PriceAppInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await priceApplicabilityService.AddAsync(requestParams);
                logger.LogInfo($"Price Applicability {request.ModuleName} added successfully.");
                return Ok(new { message = "Price Applicability added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Price Applicability", ex);
                return StatusCode(500, "An error occurred while adding the Price Applicability.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PriceAppUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<PriceAppUpdateRequest, PriceAppUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await priceApplicabilityService.UpdateAsync(requestParams);
            logger.LogInfo($"Price Applicability with id {request.RateAppId} updated successfully.");
            return Ok(new { message = "Price Applicability updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await priceApplicabilityService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Price Applicability with id {id} deleted successfully.");
                return Ok(new { message = "Price Applicability deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Price Applicability with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Price Applicability.");
            }
        }


    }
}
