    using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Api.Models.Request.Admin.GlobleSetting.ConfigSettings;
using MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Api.Models.Request.Admin.Rejection;
using MilkMatrix.Api.Models.Request.Bank.Bank;
using MilkMatrix.Api.Models.Request.MPP;
using MilkMatrix.Core.Abstractions.Approval.Service;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Rejection;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Request.Rejection;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Admin.GlobleSetting;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Contracts.ConfigSettings;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.ConfigSettings;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.ConfigSettings;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Response.ConfigSettings;
using static MilkMatrix.Api.Common.Constants.Constants;
using InsertDetails = MilkMatrix.Core.Entities.Request.Approval.Details.Insert;
using InsertDetailsModel = MilkMatrix.Api.Models.Request.Admin.Approval.Details.InsertModel;
using InsertLevel = MilkMatrix.Core.Entities.Request.Approval.Level.Insert;
using InsertLevelModel = MilkMatrix.Api.Models.Request.Admin.Approval.Level.InsertModel;

namespace MilkMatrix.Api.Controllers.v1;

/// <summary>
/// Controller for managing administrative tasks such as user details, modules, and financial years.
/// This controller is secured and requires authorization for access.
/// </summary>
//[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logging;
    private readonly ICommonModules commonModules;
    private readonly IApprovalService approvalService;
    private readonly IRejectionService rejectionService;
    private readonly ISequenceService sequanceService;
    private readonly IConfigSettingService configSettingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="ihttpContextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="logging"></param>
    /// <param name="commonModules"></param>
    /// <param name="approvalService"></param>
    /// <param name="rejectionService"></param>
    public AdminController(IHttpContextAccessor ihttpContextAccessor,
        IMapper mapper,
        ILogging logging,
        ICommonModules commonModules,
        IApprovalService approvalService, IRejectionService rejectionService, ISequenceService sequenceService, IConfigSettingService configSettingService)
    {
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.ihttpContextAccessor = ihttpContextAccessor ?? throw new ArgumentNullException(nameof(ihttpContextAccessor));
        this.commonModules = commonModules;
        this.logging = logging.ForContext("ServiceName", nameof(AdminController));
        this.approvalService = approvalService;
        this.rejectionService = rejectionService;
        this.sequanceService= sequenceService;
        this.configSettingService = configSettingService ?? throw new ArgumentNullException(nameof(configSettingService));
    }

    /// <summary>
    /// Retrieves common user details based on the user.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("common-list")]
    public async Task<IActionResult> GetCommonUserDetails()
    {
        var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var mobileId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var response = await commonModules.GetCommonDetails(userId, mobileId);
        return response != null
          ? Ok(response)
          : BadRequest(new ErrorResponse
          {
              StatusCode = (int)HttpStatusCode.BadRequest,
              ErrorMessage = string.Format(ErrorMessage.NotFound)
          });
    }

    /// <summary>
    /// Retrieves a list of modules available to the user.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("module-list")]
    public async Task<IActionResult> GetModules()
    {
        var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var mobileId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var response = await commonModules.GetModulesAsync(userId, mobileId);
        return response != null
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }

    /// <summary>
    /// Retrieves a list of financial years based on the provided request.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("financial-year-list")]
    public async Task<IActionResult> GetFinancialYearList(FinancialYearModel request)
    {
        var result = await commonModules.GetFinancialYearAsync(mapper.Map<FinancialYearRequest>(request));
        return result != null && result.Any() ? Ok(result) : NotFound("No records found");
    }

    /// <summary>
    /// Retrieves a list of actions available.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("action-list")]
    public async Task<IActionResult> GetActions([FromQuery] int? id = null)
    {
        var response = await commonModules.GetActionDetailsAsync(id);
        return response != null && response.Any()
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }

    #region Approval level

    [HttpGet("approval-level/{pageId}/{businessId}")]
    public async Task<ActionResult> GetById(int pageId, int businessId)
    {
        try
        {
            var response = await approvalService.GetByIdAsync(pageId, businessId);
            if (response == null)
            {
                logging.LogInfo($"details with page {pageId} and businessid {businessId} not found.");
                return NoContent();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving details with id: {pageId} and businessId {businessId}", ex);
            return StatusCode(500, "An error occurred while retrieving the details.");
        }
    }

    [HttpPost("approval-level-insert")]
    public async Task<IActionResult> InsertApprovalLevels([FromBody] IEnumerable<InsertLevelModel> request)
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
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<IEnumerable<InsertLevel>,IEnumerable<InsertLevelModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await approvalService.AddAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("approval-level-list")]
    public async Task<IActionResult> List([FromBody] ListsRequest request)
    {
        var result = await approvalService.GetAllAsync(request);
        return Ok(result);
    }
    #endregion

    #region Approval details
    [HttpPost("approval-details-insert")]
    public async Task<IActionResult> InsertApprovalDetails([FromBody]IEnumerable<InsertDetailsModel> request)
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
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions<IEnumerable<InsertDetails>,IEnumerable<InsertDetailsModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await approvalService.AddDetailsAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("approval-details-list")]
    public async Task<IActionResult> DetailsList([FromBody] ListsRequest request)
    {
        var result = await approvalService.GetAllDetailsAsync(request);
        return Ok(result);
    }

    [HttpGet("page-approval-details")]
    public async Task<IActionResult> GetPageApprovalDetails([FromQuery] int pageId, int businessId, string recordId)
    {
        try
        {
            var response = await approvalService.GetPageApprovalDetailsAsync(pageId, businessId, recordId);
            if (response == null)
            {
                logging.LogInfo($"No approval details found for page {pageId}, business {businessId}, record {recordId}.");
                return NoContent();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving approval details for page {pageId}, business {businessId}, record {recordId}.", ex);
            return StatusCode(500, "An error occurred while retrieving the approval details.");
        }
    }
    #endregion

    #region Rejection

    [HttpPost("rejection-insert")]
    public async Task<IActionResult> InsertRejectionDetails([FromBody]IEnumerable<RejectionModel> request)
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
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var requestParams = mapper.MapWithOptions< IEnumerable<InsertRejection>, IEnumerable<RejectionModel>>(request
                , new Dictionary<string, object> {
            { Constants.AutoMapper.CreatedBy ,Convert.ToInt32(UserId)}
            });
            await rejectionService.AddAsync(requestParams);
            return Ok(new { message = "Success." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in insert details", ex);
            return StatusCode(500, "An error occurred while processing the details.");
        }
    }

    [HttpPost("rejection-list")]
    public async Task<IActionResult> RejectionList([FromBody] ListsRequest request)
    {
        var result = await rejectionService.GetAllAsync(request);
        return Ok(result);
    }
    #endregion

    #region Sequence

    [HttpPost("sequence-list")]
    public async Task<IActionResult> GetSequanceList([FromBody] ListsRequest request)
    {
        var result = await sequanceService.GetSequanceList(request);
        return Ok(result);
    }

    
    [HttpGet("Sequence-list-id")]
    public async Task<ActionResult<SequenceResponse?>> GetSequanceById(string HeadName)
    {
        try
        {
            //logger.LogInfo($"GetSequanceById called for Sequence ID: {id}");
            logging.LogError($"GetSequanceById called for Sequence ID: {HeadName}");

            var result = await sequanceService.GetSequanceById(HeadName);
            if (result == null)
            {
                //logger.LogInfo($"Sequence with ID {id} not found.");
                logging.LogError($"Sequence with ID {HeadName} not found.");
                return NotFound();
            }

            
            logging.LogError($"Sequence with ID {HeadName} retrieved successfully.");
            return Ok(result);
        }
        catch (Exception ex)
        {
          
            logging.LogError($"Error retrieving Sequence with ID: {HeadName}", ex);

            return StatusCode(500, "An error occurred while retrieving the record. " + ex);
        }
    }

    [HttpPost("insert-sequence")]
    public async Task<IActionResult> Insertsequence([FromBody] SequanceInsertRequestModel request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid request."
                });
            }

            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            logging.LogInfo($"Insertsequence called for Sequence: {request.HeadName}");

            var mappedRequest = mapper.MapWithOptions<SequenceInsertRequest, SequanceInsertRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                });

            await sequanceService.Insertsequence(mappedRequest);
            return Ok(new { message = "Sequence inserted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Insertsequence", ex);
            return StatusCode(500, "An error occurred while inserting the record. " + ex);
        }
    }

    [HttpPut("update-sequence")]
    public async Task<IActionResult> Updatesequence([FromBody] SequanceUpdateRequestModel request)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.HeadName))
                return BadRequest("Invalid request.");

            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var mappedRequest = mapper.MapWithOptions<SequenceUpdateRequest, SequanceUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                });

            await sequanceService.Updatesequence(mappedRequest);
            logging.LogInfo($"Sequence with  {request.HeadName} updated successfully.");
            return Ok(new { message = "Sequence " + request.HeadName + " updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Updatesequence", ex);
            return StatusCode(500, "An error occurred while updating the record. " + ex);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteSequance(int id)
    {
        try
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await sequanceService.DeleteSequance(id, Convert.ToInt32(userId));
            logging.LogError($"Sequence with ID {id} deleted successfully.");
            return Ok(new { message = "Sequence deleted successfully." });
        }
        catch (Exception ex)
        {
        
            logging.LogError($"Error in DeleteSequance with ID: {id}", ex);
            return StatusCode(500, "An error occurred while deleting the record. " + ex);
        }
    }


    [HttpPost("generatenextseq/{HeadName}")]
    public async Task<IActionResult> GenerateNextSequance(string HeadName)
    {
        try
        {

            NextNumberResponse GetUpdatedSeq = await sequanceService.GetNextNumberforSeq(HeadName);
            logging.LogError($"Sequence with ID {HeadName} deleted successfully.");
            return Ok(new { GetUpdatedSeq });
        }
        catch (Exception ex)
        {

            logging.LogError($"Error in DeleteSequance with ID: {HeadName}", ex);
            return StatusCode(500, "An error occurred while deleting the record. " + ex);
        }
    }

    #endregion

    #region SequenceTrans

    [HttpPost("sequenceTrans-list")]
    public async Task<IActionResult> GetSequanceTransList([FromBody] ListsRequest request)
    {
        var result = await sequanceService.GetSequanceTransList(request);
        return Ok(result);
    }


    [HttpGet("SequenceTrans-list-id")]
    public async Task<ActionResult<SequenceResponse?>> GetSequanceTransById(string HeadName, string FY )
    {
        try
        {
            //logger.LogInfo($"GetSequanceById called for Sequence ID: {id}");
            logging.LogError($"GetSequanceById called for Sequence ID: {HeadName}");

            var result = await sequanceService.GetSequanceTransById(HeadName,FY);
            if (result == null)
            {
                //logger.LogInfo($"Sequence with ID {id} not found.");
                logging.LogError($"Sequence with ID {HeadName},{FY} not found.");
                return NotFound();
            }


            logging.LogError($"Sequence with ID {HeadName},{FY} retrieved successfully.");
            return Ok(result);
        }
        catch (Exception ex)
        {

            logging.LogError($"Error retrieving Sequence with ID: {HeadName},{FY}", ex);

            return StatusCode(500, "An error occurred while retrieving the record. " + ex);
        }
    }

    [HttpPost("insert-sequencetrans")]
    public async Task<IActionResult> InsertsequenceTrans([FromBody] SequanceTransInsertRequestModel request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid request."
                });
            }

            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            logging.LogInfo($"Insertsequence called for Sequence: {request.HeadName}");

            var mappedRequest = mapper.MapWithOptions<SequenceTransInsertRequest, SequanceTransInsertRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                });

            await sequanceService.InsertsequenceTrans(mappedRequest);
            return Ok(new { message = "Sequence inserted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Insertsequence", ex);
            return StatusCode(500, "An error occurred while inserting the record. " + ex);
        }
    }

    [HttpPut("update-sequencetrans")]
    public async Task<IActionResult> UpdatesequenceTrans([FromBody] SequanceTransUpdateRequestModel request)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.HeadName))
                return BadRequest("Invalid request.");

            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var mappedRequest = mapper.MapWithOptions<SequenceTransUpdateRequest, SequanceTransUpdateRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                });

            await sequanceService.UpdatesequenceTrans(mappedRequest);
            logging.LogInfo($"Sequence with  {request.HeadName} updated successfully.");
            return Ok(new { message = "Sequence " + request.HeadName + " updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in Updatesequence", ex);
            return StatusCode(500, "An error occurred while updating the record. " + ex);
        }
    }

  

    [HttpPost("generatenextseqtrans/{HeadName}")]
    public async Task<IActionResult> GenerateNextSequanceTrans(string HeadName, string FY)
    {
        try
        {

            SeqTransNextNumberResponse GetUpdatedSeq = await sequanceService.GetNextNumberforSeqTrans(HeadName,FY);
            logging.LogError($"Sequence  Next No with ID {HeadName} Generated successfully.");
            return Ok(new { GetUpdatedSeq });
        }
        catch (Exception ex)
        {

            logging.LogError($"Error in Generated with ID: {HeadName}", ex);
            return StatusCode(500, "An error occurred while geneated Next No the record. " + ex);
        }
    }


    [HttpPost("SeqTransCloneforAllDocs/{clonefromfy}/{newfy}")]
    public async Task<IActionResult> SeqTransCloneforAllDocs(string clonefromfy, string newfy)
    {
        try
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            SequenceTransResponse newclonedata = await sequanceService.SeqTransCloneforAllDocs(clonefromfy, newfy, Convert.ToInt32(userId));
            logging.LogError($"Sequence Trans Clone from  {clonefromfy} to {newfy} Generated successfully.");
            return Ok(new { newclonedata });
        }
        catch (Exception ex)
        {

            logging.LogError($"Error Sequence Trans Clone from  {clonefromfy} to {newfy}", ex);
            return StatusCode(500, "An error occurred while Sequence Trans Clone from  {clonefromfy} to {newfy} " + ex);
        }
    }


    [HttpPost("SeqTransCloneforSelHead/{clonefromfy}/{clonefromhead}/{newfy}")]
    public async Task<IActionResult> SeqTransCloneforselective(string clonefromfy, string clonefromhead, string newfy )
    {
        try
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            SequenceTransResponse newclonedata = await sequanceService.SeqTransCloneforSelectiveHead(clonefromfy,clonefromhead, newfy, Convert.ToInt32(userId));
            logging.LogError($"Sequence Trans Clone from  {clonefromfy} and {clonefromhead} to {newfy} and {clonefromhead} Generated successfully.");
            return Ok(new { newclonedata });
        }
        catch (Exception ex)
        {

            logging.LogError($"Error Sequence Trans Clone from  {clonefromfy}  and {clonefromhead} to {newfy}  and {clonefromhead} ", ex);
            return StatusCode(500, "An error occurred while Sequence Trans Clone from  {clonefromfy}  and {clonefromhead} to {newfy}  and {clonefromhead} " + ex);
        }
    }

    #endregion

    #region ConfigSetting

    [HttpPost("config-setting-list")]
    public async Task<IActionResult> GetCompanySettings([FromBody] ListsRequest request)
    {
        var result = await configSettingService.GetAll(request);
        return Ok(result);
    }

    [HttpGet("config-setting-{businessId}")]
    public async Task<ActionResult<ConfigSettingResponse?>> GetCompanySettingById(int businessId, string unitType, int unitIds)
    {
        try
        {
            logging.LogInfo($"Get Company Setting by id called for id: {businessId}");
            var setting = await configSettingService.GetById(businessId, unitType, unitIds);
            if (setting == null)
            {
                logging.LogInfo($"Company Setting with id {businessId} not found.");
                return NotFound();
            }
            logging.LogInfo($"Company Setting with id {businessId} retrieved successfully.");
            return Ok(setting);
        }
        catch (Exception ex)
        {
            logging.LogError($"Error retrieving Company Setting with id: {businessId}", ex);
            return StatusCode(500, "An error occurred while retrieving the Company Setting.");
        }
    }

    [HttpPost("insert-config-settings")]
    public async Task<IActionResult> InsertConfigSettings([FromBody] ConfigSettingInsertRequestModel request)
    {
        try
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid request."
                });
            }

            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            logging.LogInfo($"InsertConfigSettings called for Key: {request}");

            var mappedRequest = mapper.MapWithOptions<ConfigSettingInsertRequest, ConfigSettingInsertRequestModel>(
                request,
                new Dictionary<string, object>
                {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                });

            await configSettingService.InsertConfigSetting(mappedRequest);
            return Ok(new { message = "Config settings inserted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError("Error in InsertConfigSettings", ex);
            return StatusCode(500, "An error occurred while inserting the record. " + ex.Message);
        }
    }


    [HttpPost("config-setting-Update")]
    public async Task<IActionResult> UpdateCompanySetting([FromBody] ConfigSettingUpdateRequestModel request)
    {
        try
        {
            if (!ModelState.IsValid || request.BusinessId <= 0)
                return BadRequest("Invalid request.");
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var requestParams = mapper.MapWithOptions<ConfigSettingUpdateRequest, ConfigSettingUpdateRequestModel>(
               request,
               new Dictionary<string, object>
               {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt32(userId) }
               });
            await configSettingService.UpdateConfigSetting(requestParams);
            logging.LogInfo($"Company Setting with id {request.BusinessId} updated successfully.");
            return Ok(new { message = "Company Setting updated successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error updating Company Setting with id: {request.BusinessId}", ex);
            return StatusCode(500, "An error occurred while updating the Company Setting.");
        }
    }

    [HttpDelete("config-setting-delete/{BusinessId}")]
    public async Task<IActionResult> DeleteCompanySetting(int BusinessId, string UnitType, string UnitIds)
    {
        try
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            await configSettingService.DeleteConfigSetting(BusinessId, UnitType, UnitIds);
            logging.LogInfo($"Company Setting with id {BusinessId} deleted successfully.");
            return Ok(new { message = "Company Setting deleted successfully." });
        }
        catch (Exception ex)
        {
            logging.LogError($"Error deleting Company Setting with id: {BusinessId}", ex);
            return StatusCode(500, "An error occurred while deleting the Company Setting.");
        }

    }

    #endregion
}
