using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Bank.BankRegional;
using MilkMatrix.Api.Models.Request.Bank.BankType;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
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
    public class BankController :  ControllerBase
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IBankRegService bankRegService;
        private readonly IBankTypeService bankTypeService;
        private readonly IMapper mapper;
        public BankController(IHttpContextAccessor httpContextAccessor, ILogging logging, IBankRegService bankRegService, IBankTypeService bankTypeService,IMapper mapper)
        {
            // Constructor logic if needed
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(GeographicalController)) ?? throw new ArgumentNullException(nameof(logging));
            this.bankRegService = bankRegService ?? throw new ArgumentNullException(nameof(bankRegService));
            this.bankTypeService = bankTypeService ?? throw new ArgumentNullException(nameof(bankTypeService));
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("regional-bank-list")]
        public async Task<IActionResult> GetRegionalBank([FromBody] BankRegionalModel request)
        {
            logger.LogInfo($"GetRegionalBanks request processed with ActionType: {request.ActionType}, " +
                           $"RegionalId: {request.RegionalID}");

            var regionalRequest = new BankRegionalRequest
            {
                BankRegionalId = request.RegionalID,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };

            var response = regionalRequest.ActionType == ReadActionType.All
                ? await bankRegService.GetBankReg(regionalRequest)
                : await bankRegService.GetSpecificLists(regionalRequest);

            return response != null && response.Any()
                ? Ok(response)
                : BadRequest("No records found.");
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

        [HttpPost]
        [Route("bankType-list")]
        public async Task<ActionResult> GetBankType([FromBody] BankTypeRequestModel request)
        {
            logger.LogInfo($"GetBankType request processed with ActionType: {request.ActionType}, " +
                $"BankTypeId: {request.BankTypeId}, StateId: {request.BankTypeId}, " +
                $"DistrictId: {request.BankTypeId}");

            var bankTypeReq = new BankTypeRequest
            {
                BankTypeId = request.BankTypeId,
                ActionType = (ReadActionType)request.ActionType,
                IsActive = true
            };
            var response = request.ActionType == ReadActionType.All
                 ? await bankTypeService.GetBankTypes(bankTypeReq)
                : await bankTypeService.GetSpecificLists(bankTypeReq);

            return response.Any() ? Ok(response) : BadRequest("No bank type data found.");
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


    }
}
