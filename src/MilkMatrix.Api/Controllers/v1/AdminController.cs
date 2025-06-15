using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Domain.Entities.Responses;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAuth iAuthentication;
    private readonly ICommonModules commonModules;
    private readonly IHttpContextAccessor ihttpContextAccessor;
    private readonly IMapper mapper;
    private ILogging logger;

    public AdminController(IAuth iAuthentication, IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logger, ICommonModules commonModules)
    {
        this.iAuthentication = iAuthentication;
        this.ihttpContextAccessor = ihttpContextAccessor;
        this.mapper = mapper;
        this.commonModules = commonModules;
        this.logger = logger.ForContext("ServiceName", nameof(AdminController));
    }

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

    [HttpPost]
    [Route("module-list")]
    public async Task<IActionResult> GetModules()
    {
        var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        var mobileId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var response =await commonModules.GetModulesAsync(userId, mobileId);
        return response != null
            ? Ok(response)
            : BadRequest(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = string.Format(ErrorMessage.NotFound)
            });
    }
}
