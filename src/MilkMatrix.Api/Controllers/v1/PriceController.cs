using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Price;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Uploader;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Price;
using MilkMatrix.Milk.Models.Request.Price;
using MilkMatrix.Milk.Models.Response.Price;
using static MilkMatrix.Api.Common.Constants.Constants;
using Constants = MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class PriceController : ControllerBase
    {
        private readonly IPriceService priceService;

        private ILogging logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IFileUploader fileUploader;

        public PriceController(IPriceService priceService, ILogging logger, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFileUploader fileUploader)
        {
            this.priceService = priceService;
            this.logger = logger.ForContext("ServiceName", nameof(PriceController));
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.fileUploader = fileUploader;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await priceService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MilkPriceInsertResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Milk Price by id called for id: {id}");
                var mcc = await priceService.GetByIdAsync(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Milk Price with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Milk Price with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Milk Price with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Milk Price.");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync([FromBody] MilkPriceInsertRequestModel request)
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
                logger.LogInfo($"Add called for Milk Price: {request.WithEffectDate}");
                var requestParams = mapper.MapWithOptions<MilkPriceInsertRequest, MilkPriceInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await priceService.AddAsync(requestParams);
                logger.LogInfo($"Milk Price {request.WithEffectDate} added successfully.");
                return Ok(new { message = "Milk Price added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Milk Price", ex);
                return StatusCode(500, "An error occurred while retrieving the Milk Price." + ex);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] MilkPriceUpdateRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<MilkPriceUpdateRequest, MilkPriceUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await priceService.UpdateAsync(requestParams);
            logger.LogInfo($"Milk Price with id {request.RateCode} updated successfully.");
            return Ok(new { message = "Milk Price updated successfully." });
        }

        [HttpDelete("delete/{ratecode}")]
        public async Task<IActionResult> DeleteAsync(string ratecode)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await priceService.DeleteAsync(ratecode, Convert.ToInt32(UserId));
                logger.LogInfo($"Milk Price with id {ratecode} deleted successfully.");
                return Ok(new { message = "Milk Price deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Milk Price with id: {ratecode}", ex);
                return StatusCode(500, "An error occurred while deleting the Milk Price.");
            }
        }

        [HttpGet("milk-chart{id}")]
        public async Task<ActionResult> GetMilkChartByRateCode(int id)
        {
            try
            {
                var result = await priceService.GetMilkFatChartJsonAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Milk Chart with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Milk Chart.");
            }
        }


    }
}
