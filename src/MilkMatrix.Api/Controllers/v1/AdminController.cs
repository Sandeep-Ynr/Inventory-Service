using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Response;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1;

/// <summary>
/// Controller for managing administrative tasks such as user details, modules, and financial years.
/// This controller is secured and requires authorization for access.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ICommonModules commonModules;
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logging;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="ihttpContextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="logging"></param>
    /// <param name="commonModules"></param>   
    public AdminController(IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logging, ICommonModules commonModules)
    {
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.commonModules = commonModules;
        this.logging = logging.ForContext("ServiceName", nameof(AdminController));
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
}
