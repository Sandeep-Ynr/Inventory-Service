using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Accounts.Accountgroups;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Accounts.AccountGroups;
using MilkMatrix.Milk.Contracts.Accounts;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;
using MilkMatrix.Milk.Models.Response.Accounts.AccountGroups;
using static MilkMatrix.Api.Common.Constants.Constants;
using MilkMatrix.Milk.Implementations;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IAccountGroupsService AccountGroupsService;
        private readonly IAccountLedgerService AccountLedgerService;


        public AccountsController(IHttpContextAccessor httpContextAccessor, ILogging logger, IMapper mapper, IAccountGroupsService AccountGroupsService,IAccountLedgerService AccountLedgerService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger.ForContext("ServiceName", nameof(AccountsController)) ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.AccountGroupsService = AccountGroupsService;
            this.AccountLedgerService = AccountLedgerService;
        }

        [HttpPost]
        [Route("list-accountgroups")]
        public async Task<IActionResult> List([FromBody] ListsRequest request)
        {
            var result = await AccountGroupsService.GetAccountGroupList(request);
            return Ok(result);
        }

        [HttpGet("accountgroups{id}")]
        public async Task<ActionResult<AccountGroupsResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Account-group by id called for id: {id}");
                var mcc = await AccountGroupsService.GetAccountGroupById(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Account-group with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Account-group with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Account-group with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Account-group.");
            }
        }

        [HttpPost]
        [Route("add-accountgroups")]
        public async Task<IActionResult> AddAsync([FromBody] AccountGroupsAGInsertRequestModel request)
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

                logger.LogInfo($"Add called for Group name: {request.Name}");
                var requestParams = mapper.MapWithOptions<AccountGroupsInsertRequest, AccountGroupsAGInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });



                await AccountGroupsService.InsertAccountGroup(requestParams);
                logger.LogInfo($"Account Group {request.Name} added successfully.");
                return Ok(new { message = "Account Group added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Account Group", ex);
                return StatusCode(500, "An error occurred while adding the Account Group.");
            }
        }

        [HttpPut]
        [Route("update-accountgroups/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AccountGroupsUpdateRequestModel request)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid request.");

            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<AccountGroupsUpdateRequest, AccountGroupsUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await AccountGroupsService.UpdateAccountGroup( id, requestParams);
            logger.LogInfo($"Account Group with id {request.Name} updated successfully.");
            return Ok(new { message = "Account Group updated successfully." });
        }

        [HttpDelete("delete-accountgroups/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await AccountGroupsService.DeleteAccountGroupById(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Account Gruop with id {id} deleted successfully.");
                return Ok(new { message = "Account Gruop deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Account Gruop with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting Account Gruop .");
            }
        }

        [HttpPost]
        [Route("add-accountledger")]
        public async Task<IActionResult> AddAccountLedger([FromBody] AccountHeadsInsertRequestModel request)
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

                logger.LogInfo($"Add called for Ledger name: {request.Name}");
                var requestParams = mapper.MapWithOptions<AccountHeadsInsertRequest, AccountHeadsInsertRequestModel>(request
                    , new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await AccountLedgerService.InsertAccountLedger(requestParams);
                logger.LogInfo($"Account Ledger {request.Name} added successfully.");
                return Ok(new { message = "Account Ledger added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Account Ledger", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("update-accountledger/{id}")]
        public async Task<IActionResult> UpdateAccountLedger(int id, [FromBody] AccountHeadsUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || id <= 0)
                    return BadRequest("Invalid request.");

                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<AccountHeadsUpdateRequest, AccountHeadsUpdateRequestModel>(request
                            , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                        });
                await AccountLedgerService.UpdateAccountLedger(id, requestParams);
                logger.LogInfo($"Account ledger with id {request.Name} updated successfully.");
                return Ok(new { message = "Account ledger updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Account-Ledger with id: {id}", ex);
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPost]
        [Route("list-accountledger")]
        public async Task<IActionResult> AccountLedgerList([FromBody] ListsRequest request)
        {
            var result = await AccountLedgerService.GetAccountHeadList(request);
            return Ok(result);
        }

        [HttpGet("accountledger{id}")]
        public async Task<ActionResult<AccountGroupsResponse?>> GetAccountLedgerById(int id)
        {
            try
            {
                logger.LogInfo($"Get Account-Ledger by id called for id: {id}");
                var mcc = await AccountLedgerService.GetAccountHeadById(id);
                if (mcc == null)
                {
                    logger.LogInfo($"Account-Ledger with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Account-Ledger with id {id} retrieved successfully.");
                return Ok(mcc);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Account-Ledger with id: {id}", ex);
                return StatusCode(500,  ex.Message);
            }
        }

        [HttpDelete("delete-accountledger/{id}")]
        public async Task<IActionResult> DeleteAccountledger(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await AccountLedgerService.DeleteAccountHeadById(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Account Ledger with id {id} deleted successfully.");
                return Ok(new { message = "Account Ledger deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Account Ledger with id: {id}", ex);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
