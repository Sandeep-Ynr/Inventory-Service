using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Bank.Bank;
using MilkMatrix.Api.Models.Request.Bank.BankRegional;
using MilkMatrix.Api.Models.Request.Bank.BankType;
using MilkMatrix.Api.Models.Request.Bank.Branch;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IBankRegService bankRegService;
        private readonly IBankTypeService bankTypeService;
        private readonly IBankService bankService;
        private readonly IBranchService branchService;
        private readonly IMapper mapper;
        public BankController(IHttpContextAccessor httpContextAccessor, ILogging logging, IBankRegService bankRegService,
            IBankTypeService bankTypeService, IBankService bankService, IBranchService branchService, IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.bankRegService = bankRegService ?? throw new ArgumentNullException(nameof(bankRegService));
            this.bankTypeService = bankTypeService ?? throw new ArgumentNullException(nameof(bankTypeService));
            this.bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            this.branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
            this.mapper = mapper;
        }

        #region Bank-Regional
        [HttpPost]
        [Route("regional-list")]
        public async Task<IActionResult> regionalList([FromBody] ListsRequest request)
        {
            var result = await bankRegService.GetAllAsync(request);
            return Ok(result);
        }


        [HttpGet("bankregional/{id}")]
        public async Task<ActionResult<BankRegResponse>> GetBankRegionalById(int id)
        {
            try
            {
                logger.LogInfo($"Get Bank Regional by ID called: {id}");

                var regionalBank = await bankRegService.GetById(id);

                if (regionalBank == null)
                {
                    logger.LogInfo($"Bank Regional with ID {id} not found.");
                    return NotFound(new { message = $"Bank Regional with ID {id} not found." });
                }

                logger.LogInfo($"Bank Regional with ID {id} retrieved successfully.");
                return Ok(regionalBank);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Bank Regional with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Bank Regional.");
            }
        }

        [HttpPost]
        [Route("add-bankRegional")]
        public async Task<IActionResult> AddBankRegional([FromBody] BankRegInsertReqModel request)
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
                logger.LogInfo($"Add called for Bank Regional: {request.RegionalBankName}");
                var requestParams = mapper.MapWithOptions<BankRegInsertRequest, BankRegInsertReqModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await bankRegService.AddBankReg(requestParams);
                logger.LogInfo($"Hamlet {request.RegionalBankName} added successfully.");
                return Ok(new { message = "Bank Regional added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert Bank Regional", ex);
                return StatusCode(500, "An error occurred while adding the RegionalBank.");
            }
        }

        [HttpPut]
        [Route("update-bankRegional")]
        public async Task<IActionResult> UpdateBankRegional([FromBody] BankRegUpdateReqModel request)
        {
            if (!ModelState.IsValid || request.RegionalID <= 0)
                return BadRequest("Invalid request.");
            var UserId = 3;// httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            logger.LogInfo($"Update called for Bank Regional ID: {request.RegionalID}");
            var requestParams = mapper.MapWithOptions<BankRegUpdateRequest, BankRegUpdateReqModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await bankRegService.UpdateBankReg(requestParams);
            logger.LogInfo($"Bank Regional with ID {request.RegionalCode} updated successfully.");
            return Ok(new { message = "Bank Regional updated successfully." });
        }
        [HttpDelete("delete-bankRegional/{id}")]
        public async Task<IActionResult> DeleteBankRegional(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await bankRegService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Bank Regional with id {id} deleted successfully.");
                return Ok(new { message = "Bank Regional deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Bank Regional with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Hamlet.");
            }
        }
        #endregion


        #region Bank-Type
        [HttpPost]
        [Route("bankType-list")]
        public async Task<IActionResult> BankTypeList([FromBody] ListsRequest request)
        {
            var result = await bankTypeService.GetAll(request);
            return Ok(result);
        }



        [HttpGet("bankTypeID{id}")]
        public async Task<ActionResult<BankTypeResponse?>> GetByBankTypeId(int id)
        {
            try
            {
                logger.LogInfo($"Get Bank Type by id called for id: {id}");
                var user = await bankTypeService.GetByBankTypeId(id);
                if (user == null)
                {
                    logger.LogInfo($"Bank Type with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Bank Type with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Bank Type with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Bank Type.");
            }
        }

        [HttpPost]
        [Route("add-bankType")]
        public async Task<IActionResult> AddBankType([FromBody] BankTypeInsertRequestModel request)
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
                logger.LogInfo($"Add called for Bank Type: {request.BankTypeName}");
                var requestParams = mapper.MapWithOptions<BankTypeInsertRequest, BankTypeInsertRequestModel>(request
                    , new Dictionary<string, object> {
                                { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
                });
                await bankTypeService.AddBankType(requestParams);
                logger.LogInfo($"Bank Type {request.BankTypeName} added successfully.");
                return Ok(new { message = "Bank Type added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Upsert BankType", ex);
                return StatusCode(500, "An error occurred while adding the Hamlet.");
            }
        }

        [HttpPut]
        [Route("update-bankType")]
        public async Task<IActionResult> UpdateBankType([FromBody] BankTypeUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.BankTypeId <= 0)
                return BadRequest("Invalid request.");
            var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<BankTypeUpdateRequest, BankTypeUpdateRequestModel>(request
                        , new Dictionary<string, object> {
                            {Constants.AutoMapper.ModifiedBy ,Convert.ToInt32(UserId)}
                    });
            await bankTypeService.UpdateBankType(requestParams);
            logger.LogInfo($"Bank Type with id {request.BankTypeId} updated successfully.");
            return Ok(new { message = "Bank Type updated successfully." });
        }

        [HttpDelete("bankType-delete/{id}")]
        public async Task<IActionResult> DeleteBankType(int id)
        {
            try
            {
                var UserId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await bankTypeService.DeleteAsync(id, Convert.ToInt32(UserId));
                logger.LogInfo($"Hamlet with id {id} deleted successfully.");
                return Ok(new { message = "Bank Type deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Hamlet with id: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Bank Type.");
            }
        }
        #endregion


        #region Bank-Master
        [HttpPost]
        [Route("bank-list")]
        public async Task<IActionResult> BankList([FromBody] ListsRequest request)
        {
            var result = await bankService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("bankID{id}")]
        public async Task<ActionResult<BankResponse?>> GetByBankId(int id)
        {
            try
            {
                logger.LogInfo($"Get Bank by ID called for ID: {id}");
                var bank = await bankService.GetById(id);
                if (bank == null)
                {
                    logger.LogInfo($"Bank with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Bank with ID {id} retrieved successfully.");
                return Ok(bank);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Bank with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Bank.");
            }
        }

        [HttpPost]
        [Route("add-bank")]
        public async Task<IActionResult> AddBank([FromBody] BankInsertRequestModel request)
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

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                logger.LogInfo($"Add called for Bank: {request.BankName}");

                var requestParams = mapper.MapWithOptions<BankInsertRequest, BankInsertRequestModel>(request,
                    new Dictionary<string, object>{
                              { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await bankService.AddBank(requestParams);
                logger.LogInfo($"Bank {request.BankName} added successfully.");
                return Ok(new { message = "Bank added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding Bank", ex);
                return StatusCode(500, "An error occurred while adding the Bank.");
            }
        }

        [HttpPut]
        [Route("update-bank")]
        public async Task<IActionResult> UpdateBank([FromBody] BankUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.BankID <= 0)
                return BadRequest("Invalid request.");

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<BankUpdateRequest, BankUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                });

            await bankService.UpdateBank(requestParams);
            logger.LogInfo($"Bank with ID {request.BankID} updated successfully.");
            return Ok(new { message = "Bank updated successfully." });
        }

        [HttpDelete("bank-delete/{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await bankService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Bank with ID {id} deleted successfully.");
                return Ok(new { message = "Bank deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Bank with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Bank.");
            }
        }
        #endregion





        #region Bank-Branch
        [HttpPost]
        [Route("branch-list")]
        public async Task<IActionResult> BranchList([FromBody] ListsRequest request)
        {
            var result = await branchService.GetAll(request);
            return Ok(result);
        }
        #endregion

        [HttpGet("branchID{id}")]
        public async Task<ActionResult<BranchResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"Get Bank Branch by id called for id: {id}");
                var user = await branchService.GetByBranchId(id);
                if (user == null)
                {
                    logger.LogInfo($"Bank Branch with id {id} not found.");
                    return NotFound();
                }
                logger.LogInfo($"Bank Branch with id {id} retrieved successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Bank Branch with id: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the Bank Branch.");
            }
        }

        [HttpPost]
        [Route("add-branch")]
        public async Task<IActionResult> AddBranch([FromBody] BranchInsertRequestModel request)
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
                logger.LogInfo($"Add called for Branch: {request.BranchName}");

                var requestParams = mapper.MapWithOptions<BranchInsertRequest, BranchInsertRequestModel>(
                    request,
                    new Dictionary<string, object>{
                        { Constants.AutoMapper.CreatedBy, Convert.ToInt32(userId) }
                    });

                await branchService.AddBranch(requestParams);

                logger.LogInfo($"Branch {request.BranchName} added successfully.");
                return Ok(new { message = "Branch added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in AddBranch", ex);
                return StatusCode(500, "An error occurred while adding the Branch.");
            }
        }

        [HttpPut]
        [Route("update-branch")]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchUpdateRequestModel request)
        {
            if (!ModelState.IsValid || request.BranchId <= 0)
            {
                return BadRequest(new { message = "Invalid request." });
            }
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                logger.LogInfo($"Update called for Branch ID: {request.BranchId}");

                var requestParams = mapper.MapWithOptions<BranchUpdateRequest, BranchUpdateRequestModel>(
                    request,new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
                    });

                await branchService.UpdateBranch(requestParams);
                logger.LogInfo($"Branch with ID {request.BranchId} updated successfully.");
                return Ok(new { message = "Branch updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating Branch ID: {request.BranchId}", ex);
                return StatusCode(500, "An error occurred while updating the Branch.");
            }
        }

        [HttpDelete("branch-delete/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await branchService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"Branch with ID {id} deleted successfully.");
                return Ok(new { message = "Branch deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Branch with ID: {id}", ex);
                return StatusCode(500, "An error occurred while deleting the Branch.");
            }
        }




    }
}
