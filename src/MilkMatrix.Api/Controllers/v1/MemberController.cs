using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Member;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Contracts.Member;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Response.Member;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IMemberService memberService;
        public MemberController(IHttpContextAccessor httpContextAccessor, IMemberService memberService, ILogging logging, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            this.mapper = mapper;
        }

        #region Member

        [HttpPost("list-member")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await memberService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("member/{id}")]
        public async Task<ActionResult<MemberResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for Member ID: {id}");
                var result = await memberService.GetById(id);
                if (result == null)
                {
                    logger.LogInfo($"Member with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Member with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Member with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> Add([FromBody] MemberInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = ErrorMessage.InvalidRequest
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                

                var mappedRequest = mapper.MapWithOptions<MemberInsertRequest, MemberInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
               { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });
                

                await memberService.AddMember(mappedRequest);
                //logger.LogInfo($"Member {request.FarmerName} added successfully.");
                return Ok(new { message = "Member added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding member", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }


        [HttpPut("update-member")]
        public async Task<IActionResult> Update([FromBody] MemberUpdateRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = ErrorMessage.InvalidRequest
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var mappedRequest = mapper.MapWithOptions<MemberUpdateRequest, MemberUpdateRequestModel>(
                    request,new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });
                await memberService.UpdateMember(mappedRequest);
                logger.LogInfo($"Member {request.MemberID} updated successfully.");
                return Ok(new { message = "Member updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating member", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }

        [HttpDelete("delete/{memberId}")]
        public async Task<IActionResult> Delete(int memberId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await memberService.Delete(memberId, Convert.ToInt32(userId));
                logger.LogInfo($"Member with ID {memberId} deleted successfully.");
                return Ok(new { message = "Member deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Member with ID: {memberId}", ex);
                return StatusCode(500, "An error occurred while deleting the member. " + ex.Message);
            }
        }

        #endregion




    }
}
