using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Party;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Party;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Party;
using MilkMatrix.Milk.Models.Response.Party;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IPartyGroupService partyGroupService;
        private readonly IPartyService partyService;

        public PartyController(IHttpContextAccessor httpContextAccessor, ILogging logging, IPartyService partyService, IPartyGroupService partyGroupService, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
            this.partyGroupService = partyGroupService ?? throw new ArgumentNullException(nameof(partyGroupService));
            this.mapper = mapper;
        }

        #region PartyGroup
        [HttpPost("group/list")]
        public async Task<IActionResult> GetGroupList([FromBody] ListsRequest request)
        {
            try
            {
                var result = await partyGroupService.GetAll(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving party group list", ex);
                return StatusCode(500, "An error occurred while fetching the groups." + ex.Message);
            }
        }

        [HttpGet("group/id/{id}")]
        public async Task<IActionResult> GetGroupById(long id)
        {
            try
            {
                var result = await partyGroupService.GetById(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving party group", ex);
                return StatusCode(500, "An error occurred while fetching the group." + ex.Message);
            }
        }


        [HttpPost("group/add")]
        public async Task<IActionResult> AddGroup([FromBody] PartyGroupInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var requestParams = mapper.MapWithOptions<PartyGroupInsertRequest, PartyGroupInsertRequestModel>(request,
                    new Dictionary<string, object> {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await partyGroupService.AddPartyGroup(requestParams);
                return Ok(new { message = "Party group added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding party group", ex);
                return StatusCode(500, "An error occurred while adding the group." + ex.Message);
            }
        }

        [HttpPut("group/update")]
        public async Task<IActionResult> UpdateGroup([FromBody] PartyGroupUpdateRequestModel request)
        {
            try
            {
                if (request == null || request.GroupId <= 0)
                    return BadRequest("Invalid request.");

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<PartyGroupUpdateRequest, PartyGroupUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await partyGroupService.UpdatePartyGroup(mappedRequest);
                return Ok(new { message = "Party group updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating party group", ex);
                return StatusCode(500, "An error occurred while updating the group." + ex.Message);
            }
        }

        [HttpDelete("group/delete/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await partyGroupService.DeleteById(id, Convert.ToInt32(userId));
                logger.LogInfo($"PartyGroup with ID {id} deleted successfully.");
                return Ok(new { message = "Party deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting PartyGroup with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the record." + ex);
            }
        }
        #endregion


        #region Party
        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await partyService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<PartyDetailResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for Party ID: {id}");
                var result = await partyService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Party with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Party with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Party with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record." + ex);
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] PartyInsertRequestModel request)
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
                logger.LogInfo($"Add called for Party: {request.PartyName}");
                var requestParams = mapper.MapWithOptions<PartyInsertRequest, PartyInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await partyService.AddParty(requestParams);
                logger.LogInfo($"Party {request.PartyName} added successfully.");
                return Ok(new { message = "Party added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add Party", ex);
                return StatusCode(500, "An error occurred while adding the record." + ex);
            }
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] PartyUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.PartyId <= 0)
                    return BadRequest("Invalid request.");

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<PartyUpdateRequest, PartyUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await partyService.UpdateParty(id,mappedRequest);
                logger.LogInfo($"Party with ID {request.PartyId} updated successfully.");
                return Ok(new { message = "Party updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in updating Party", ex);
                return StatusCode(500, "An error occurred while updating the record." + ex);
            }
        }

      
        #endregion

    }
}
