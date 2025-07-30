using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Member;
using MilkMatrix.Api.Models.Request.Member.MemberAddress;
using MilkMatrix.Api.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Api.Models.Request.Member.MemberDocuments;
using MilkMatrix.Api.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Member;
using MilkMatrix.Milk.Contracts.Member.MemberAddress;
using MilkMatrix.Milk.Contracts.Member.MemberBankDetails;
using MilkMatrix.Milk.Contracts.Member.MemberDocuments;
using MilkMatrix.Milk.Contracts.Member.MilkProfile;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.Member.Address;
using MilkMatrix.Milk.Implementations.Member.MemberBankDetails;
using MilkMatrix.Milk.Implementations.Member.MemberDocuments;
using MilkMatrix.Milk.Implementations.Member.MilkProfile;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Request.Member.MemberAddress;
using MilkMatrix.Milk.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Request.Member.MemberDocuments;
using MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Milk.Models.Response.Member;
using MilkMatrix.Milk.Models.Response.Member.MemberAddress;
using MilkMatrix.Milk.Models.Response.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Response.Member.MemberDocuments;
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
        private readonly IMemberDocumentsService memberDocumentsService;
        public MemberController(IHttpContextAccessor httpContextAccessor, IMemberService memberService,
            IMemberAddressService memberAddressService, IMemberBankDetailsService memberBankDetailsService,
            IMemberMilkProfileService memberMilkProfileService, IMemberDocumentsService memberDocumentsService, ILogging logging, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            this.memberAddressService = memberAddressService ?? throw new ArgumentNullException(nameof(memberAddressService));
            this.memberBankDetailsService = memberBankDetailsService ?? throw new ArgumentNullException(nameof(memberBankDetailsService));
            this.memberMilkProfileService = memberMilkProfileService ?? throw new ArgumentNullException(nameof(memberMilkProfileService));
            this.memberDocumentsService = memberDocumentsService ?? throw new ArgumentNullException(nameof(memberDocumentsService));
            this.mapper = mapper;
        }

        #region Member

        [HttpPost("list-member")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var member = await memberService.GetAll(request);
            return Ok(member);
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
                var result =  await memberService.AddMember(mappedRequest);
                
                
                
                if (request.addressList != null && request.addressList.Count > 0)
                {
                    foreach (var item in request.addressList)
                    {
                        var mappedAddressRequest = mapper.MapWithOptions<MemberAddressInsertRequest, MemberAddressInsertRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                            });
                        mappedAddressRequest.MemberID =  Convert.ToInt64(result.MemberID); 
                        await memberAddressService.AddMemberAddress(mappedAddressRequest);
                    }
                }
                if (request.bankList != null && request.bankList.Count > 0)
                {
                    foreach (var item in request.bankList)
                    {
                        var mappedBankRequest = mapper.MapWithOptions<MemberBankDetailsInsertRequest, MemberBankDetailsInsertRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                            });
                        mappedBankRequest.MemberID = Convert.ToInt64(result.MemberID);
                        await memberBankDetailsService.AddMemberBankDetails(mappedBankRequest);
                    }
                }
                if (request.MilkProfileList != null && request.MilkProfileList.Count > 0)
                {
                    foreach (var item in request.MilkProfileList)
                    {
                        var mappedMilkRequest = mapper.MapWithOptions<MemberMilkProfileInsertRequest, MemberMilkProfileInsertRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                            });
                        mappedMilkRequest.MemberID = Convert.ToInt64(result.MemberID);
                        await memberMilkProfileService.AddMemberMilkProfile(mappedMilkRequest);
                    }
                }
                if (request.MilkDocumentList != null && request.MilkDocumentList.Count > 0)
                {
                    foreach (var item in request.MilkDocumentList)
                    {
                        var mappedDocumentRequest = mapper.MapWithOptions<MemberDocumentsInsertRequest, MemberDocumentsInsertRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                            });
                        mappedDocumentRequest.MemberID = Convert.ToInt64(result.MemberID);
                        await memberDocumentsService.AddMemberDocuments(mappedDocumentRequest);
                    }
                }


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
                
                if (request.addressList != null && request.addressList.Count > 0)
                {
                    foreach (var item in request.addressList)
                    {
                        var mappedAddressUpdRequest = mapper.MapWithOptions<MemberAddressUpdateRequest, MemberAddressUpdateRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                            });
                        mappedAddressUpdRequest.MemberID = Convert.ToInt64(request.MemberID);
                        await memberAddressService.UpdateMemberAddress(mappedAddressUpdRequest);
                    }
                }

                if (request.bankList != null && request.bankList.Count > 0)
                {
                    foreach (var item in request.bankList)
                    {
                        var mappedBankUpdRequest = mapper.MapWithOptions<MemberBankDetailsUpdateRequest, MemberBankDetailsUpdateRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                            });
                        mappedBankUpdRequest.MemberID = Convert.ToInt64(request.MemberID);
                        await   memberBankDetailsService.UpdateMemberBankDetails(mappedBankUpdRequest);
                    }
                }


                if (request.MilkProfileList != null && request.MilkProfileList.Count > 0)
                {
                    foreach (var item in request.MilkProfileList)
                    {
                        var mappedMilkUpdRequest = mapper.MapWithOptions<MemberMilkProfileUpdateRequest, MemberMilkProfileUpdateRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                            });
                        mappedMilkUpdRequest.MemberID = Convert.ToInt64(request.MemberID);
                        await memberMilkProfileService.UpdateMemberMilkProfile(mappedMilkUpdRequest);
                    }
                }
                if (request.MilkDocumentList != null && request.MilkDocumentList.Count > 0)
                {
                    foreach (var item in request.MilkDocumentList)
                    {
                        var mappedDocumentUpdRequest = mapper.MapWithOptions<MemberDocumentsUpdateRequest, MemberDocumentsUpdateRequestModel>(
                            item,
                            new Dictionary<string, object>
                            {
                                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                            });
                        mappedDocumentUpdRequest.MemberID = Convert.ToInt64(request.MemberID);
                        await memberDocumentsService.UpdateMemberDocuments(mappedDocumentUpdRequest);
                    }
                }

                
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
