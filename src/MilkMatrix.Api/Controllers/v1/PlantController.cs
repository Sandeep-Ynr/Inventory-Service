using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Request.Plant;
using MilkMatrix.Milk.Models.Response.Geographical;
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

        public PlantController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("state-list")]
        public async Task<IActionResult> PlantList([FromBody] ListsRequest request)
        {
            //var result = await stateService.GetAllAsync(request);
            var result = '';
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddPlant([FromBody] PlantInsertRequest request)
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

                logger.LogInfo($"Add called for Plant: {request.DistrictName}");
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
    }
}
