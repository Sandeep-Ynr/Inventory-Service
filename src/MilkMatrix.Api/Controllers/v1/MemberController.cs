using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Member;
using MilkMatrix.Api.Models.Request.Member.MemberAddress;
using MilkMatrix.Api.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Api.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Member;
using MilkMatrix.Milk.Contracts.Member.MemberAddress;
using MilkMatrix.Milk.Contracts.Member.MemberBankDetails;
using MilkMatrix.Milk.Contracts.Member.MilkProfile;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.Member.Address;
using MilkMatrix.Milk.Implementations.Member.MemberBankDetails;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Request.Member.MemberAddress;
using MilkMatrix.Milk.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Milk.Models.Response.Member;
using MilkMatrix.Milk.Models.Response.Member.MemberAddress;
using MilkMatrix.Milk.Models.Response.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Response.Member.MemberMilkProfile;
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
        private readonly IMemberAddressService memberAddressService;
        private readonly IMemberBankDetailsService memberBankDetailsService;
        private readonly IMemberMilkProfileService memberMilkProfileService;
        public MemberController(IHttpContextAccessor httpContextAccessor, IMemberService memberService,
            IMemberAddressService memberAddressService, IMemberBankDetailsService memberBankDetailsService,
            IMemberMilkProfileService memberMilkProfileService, ILogging logging, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            this.memberAddressService = memberAddressService ?? throw new ArgumentNullException(nameof(memberAddressService));
            this.memberBankDetailsService = memberBankDetailsService ?? throw new ArgumentNullException(nameof(memberBankDetailsService));
            this.memberMilkProfileService = memberMilkProfileService ?? throw new ArgumentNullException(nameof(memberMilkProfileService));
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
                    request, new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });
                await memberService.AddMember(mappedRequest);
                logger.LogInfo($"Member {request.FarmerName} added successfully.");
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
                    request,
                    new Dictionary<string, object>
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

        #region Member Address

        [HttpPost("list-memberaddress")]
        public async Task<IActionResult> GetListMemberAddresses([FromBody] ListsRequest request)
        {
            var result = await memberAddressService.GetAllMemberAddresses(request);
            return Ok(result);
        }

        [HttpGet("memberaddress/{id}")]
        public async Task<ActionResult<MemberAddressResponse?>> GetById(long id)
        {
            try
            {
                logger.LogInfo($"GetById called for MemberAddress ID: {id}");
                var result = await memberAddressService.GetMemberAddressById(id);
                if (result == null)
                {
                    logger.LogInfo($"MemberAddress with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"MemberAddress with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MemberAddress with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }

        [HttpPost("add-memberaddress")]
        public async Task<IActionResult> Add([FromBody] MemberAddressInsertRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberAddressInsertRequest, MemberAddressInsertRequestModel>(
                   request,
                   new Dictionary<string, object>
                   {
                 { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                   });

                await memberAddressService.AddMemberAddress(mappedRequest);
                logger.LogInfo($"Member Address for Member ID added successfully.");
                return Ok(new { message = "Member Address added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding member address", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }

        [HttpPut("update-memberaddress")]
        public async Task<IActionResult> Update([FromBody] MemberAddressUpdateRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberAddressUpdateRequest, MemberAddressUpdateRequestModel>(
                   request,
                   new Dictionary<string, object>
                   {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                   });

                await memberAddressService.UpdateMemberAddress(mappedRequest);
                logger.LogInfo($"Member Address with ID {request.AddressID} updated successfully.");
                return Ok(new { message = "Member Address updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating member address", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }

        [HttpDelete("delete-memberaddress/{addressId}")]
        public async Task<IActionResult> Delete(long addressId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await memberAddressService.DeleteMemberAddress(addressId, Convert.ToInt64(userId));
                logger.LogInfo($"Member Address with ID {addressId} deleted successfully.");
                return Ok(new { message = "Member Address deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Member Address with ID: {addressId}", ex);
                return StatusCode(500, "An error occurred while deleting the member address. " + ex.Message);
            }
        }

        #endregion 

        #region Member Bank Details

        [HttpPost("list-memberbankdetails")]
        public async Task<IActionResult> GetListMemberBankDetails([FromBody] ListsRequest request)
        {
            var result = await memberBankDetailsService.GetAllMemberBankDetails(request);
            return Ok(result);
        }

        [HttpGet("memberbankdetails/{id}")]
        public async Task<ActionResult<MemberBankDetailsResponse?>> GetByIdBank(long id)
        {
            try
            {
                logger.LogInfo($"GetById called for MemberBankDetails ID: {id}");
                var result = await memberBankDetailsService.GetMemberBankDetailsById(id);
                if (result == null)
                {
                    logger.LogInfo($"MemberBankDetails with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"MemberBankDetails with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MemberBankDetails with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }

        [HttpPost("add-memberbankdetails")]
        public async Task<IActionResult> Add([FromBody] MemberBankDetailsInsertRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberBankDetailsInsertRequest, MemberBankDetailsInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                 { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await memberBankDetailsService.AddMemberBankDetails(mappedRequest);
                logger.LogInfo($"Member Bank Details for Member ID {request.MemberID} added successfully.");
                return Ok(new { message = "Member Bank Details added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding member bank details", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }

        [HttpPut("update-memberbankdetails")]
        public async Task<IActionResult> Update([FromBody] MemberBankDetailsUpdateRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberBankDetailsUpdateRequest, MemberBankDetailsUpdateRequestModel>(
                   request,
                   new Dictionary<string, object>
                   {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                   });

                await memberBankDetailsService.UpdateMemberBankDetails(mappedRequest);
                logger.LogInfo($"Member Bank Details with ID {request.BankDetailID} updated successfully.");
                return Ok(new { message = "Member Bank Details updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating member bank details", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }

        [HttpDelete("delete-memberbankdetails/{bankDetailId}")]
        public async Task<IActionResult> Deletebankdetails(long bankDetailId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await memberBankDetailsService.DeleteMemberBankDetails(bankDetailId, Convert.ToInt64(userId));
                logger.LogInfo($"Member Bank Details with ID {bankDetailId} deleted successfully.");
                return Ok(new { message = "Member Bank Details deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Member Bank Details with ID: {bankDetailId}", ex);
                return StatusCode(500, "An error occurred while deleting the member bank details. " + ex.Message);
            }
        }

        #endregion

        #region Member Milk Profile

        [HttpPost("list-membermilkprofile")]
        public async Task<IActionResult> GetListMilk([FromBody] ListsRequest request)
        {
            var result = await memberMilkProfileService.GetAllMemberMilkProfiles(request);
            return Ok(result);
        }

        [HttpGet("membermilkprofile/{id}")]
        public async Task<ActionResult<MemberMilkProfileResponse?>> GetByIdMilkProfile(long id)
        {
            try
            {
                logger.LogInfo($"GetById called for MemberMilkProfile ID: {id}");
                var result = await memberMilkProfileService.GetMemberMilkProfileById(id);
                if (result == null)
                {
                    logger.LogInfo($"MemberMilkProfile with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"MemberMilkProfile with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving MemberMilkProfile with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record. " + ex.Message);
            }
        }

        [HttpPost("add-membermilkprofile")]
        public async Task<IActionResult> Add([FromBody] MemberMilkProfileInsertRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberMilkProfileInsertRequest, MemberMilkProfileInsertRequestModel>(
                   request,
                   new Dictionary<string, object>
                   {
                 { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                   });

                await memberMilkProfileService.AddMemberMilkProfile(mappedRequest);
                logger.LogInfo($"Member Milk Profile for Member ID {request.MemberID} added successfully.");
                return Ok(new { message = "Member Milk Profile added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding member milk profile", ex);
                return StatusCode(500, "An error occurred while adding the record. " + ex.Message);
            }
        }

        [HttpPut("update-membermilkprofile")]
        public async Task<IActionResult> Update([FromBody] MemberMilkProfileUpdateRequestModel request)
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
                var mappedRequest = mapper.MapWithOptions<MemberMilkProfileUpdateRequest, MemberMilkProfileUpdateRequestModel>(
                   request,
                   new Dictionary<string, object>
                   {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                   });

                await memberMilkProfileService.UpdateMemberMilkProfile(mappedRequest);
                logger.LogInfo($"Member Milk Profile with ID {request.MilkProfileID} updated successfully.");
                return Ok(new { message = "Member Milk Profile updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating member milk profile", ex);
                return StatusCode(500, "An error occurred while updating the record. " + ex.Message);
            }
        }

        [HttpDelete("delete-membermilkprofile/{milkProfileId}")]
        public async Task<IActionResult> Deletemilkprofile(long milkProfileId)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await memberMilkProfileService.DeleteMemberMilkProfile(milkProfileId, Convert.ToInt64(userId));
                logger.LogInfo($"Member Milk Profile with ID {milkProfileId} deleted successfully.");
                return Ok(new { message = "Member Milk Profile deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Member Milk Profile with ID: {milkProfileId}", ex);
                return StatusCode(500, "An error occurred while deleting the member milk profile. " + ex.Message);
            }
        }

        #endregion
    }
}
