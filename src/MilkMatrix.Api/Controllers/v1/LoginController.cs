using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models.Login.Response;
using MilkMatrix.Api.Common.Utils;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using static Azure.Core.HttpHeader;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuth iAuthentication;
        private readonly IHttpContextAccessor ihttpContextAccessor;
        private readonly IMapper mapper;
        private ILogging logger;

        public LoginController(IAuth iAuthentication, IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logger)
        {
            this.iAuthentication = iAuthentication;
            this.ihttpContextAccessor = ihttpContextAccessor;
            this.mapper = mapper;
            this.logger = logger.ForContext("ServiceName", nameof(LoginController));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("user-login/{isLogingWithOtp}")]
        public async Task<IActionResult> AuthenticateUserLogin(bool isLogingWithOtp, LoginModel login)
        {
            if (login == null)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.GenericException, "UserId", "Password")
                });
            }
            if (!isLogingWithOtp && string.IsNullOrEmpty(login.UserId))
            {
                return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.BadRequest, ErrorMessage = string.Format(ErrorMessage.BadRequest, "UserId", "Password") });
            }
            if (isLogingWithOtp && string.IsNullOrEmpty(login.Mobile))
            {
                return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.BadRequest, ErrorMessage = string.Format(ErrorMessage.BadRequest, "UserId", "Password") });
            }
            ihttpContextAccessor.GetUserBrowserDetails(out var publicIpAddress, out var privateIpAddress, out var userAgent);
            var loginResponse = await iAuthentication.AuthenticateUserLogin(mapper.MapWithOptions<LoginRequest, LoginModel>(login
                , new Dictionary<string, object> {
                { Constants.AutoMapper.HostName ,ihttpContextAccessor?.HttpContext?.Request?.Host.ToString() },
                { Constants.AutoMapper.PrivateIp ,privateIpAddress?.ToString() },
                { Constants.AutoMapper.PublicIp ,publicIpAddress?.ToString() },
                { Constants.AutoMapper.UserAgent ,userAgent },
                { Constants.AutoMapper.IsLoginWithOtp , isLogingWithOtp }
            }));
            if (loginResponse == null)
                return NotFound();
            else
                return Ok(loginResponse);
        }

        [HttpGet("logged-in-details")]
        public async Task<ActionResult> GetUserDetails()
        {
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var response = await iAuthentication.GetUserDetailsAsync(UserId);

            return response != null && response.Any()
                ? Ok(response)
                : NotFound();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel request)
        {
            var meta = new TokenStatusResponse();
            var finalResponse = new LoginMasterResponse();
            var secToken = ihttpContextAccessor?.HttpContext?.Request?.Headers?.Where(x => x.Key == "Authorization").FirstOrDefault().Value.ToString();
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var userData = await iAuthentication.GetUserDetailsAsync(userId);
            if (userData == null || !userData.Any())
            {
                return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.Unauthorized, ErrorMessage = ErrorMessage.UnAuthorized });
            }
            var refreshTokenResponse = await iAuthentication.ValidateRefreshToken(new RefreshTokenRequest
            {
                EmailId = userData!.FirstOrDefault()!.EmailId,
                RefreshToken = request.RefreshToken,
                Token = secToken
            });
            if (refreshTokenResponse.Message != "Unauthorized")
            {
                meta = await iAuthentication.UpdateAccessToken(new RefreshTokenRequest
                {
                    EmailId = userData!.FirstOrDefault()!.EmailId,
                    RefreshToken = request.RefreshToken,
                    Token = secToken
                });
                if (meta.Message != "Unauthorized")
                {
                    finalResponse.Data = await iAuthentication.GetTokenResponseFromLoggedInUser(userData!.FirstOrDefault()!.UserId);
                    finalResponse.Message = meta.Message;
                    return Ok(finalResponse);
                }
            }
            return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.Unauthorized, ErrorMessage = ErrorMessage.UnAuthorized });
        }

        #region Forget Password
        [AllowAnonymous]
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var result = new TokenStatusResponse();
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            result = await iAuthentication.ForgotPassword(mapper.Map<ForgotPasswordRequest>(model));
            if (result.Status != HttpStatusCode.OK.ToString())
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = result.Message
                });
            }
            else
                return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("verify-forgot-password")]
        public async Task<IActionResult> VerifyForgotPassword(ResetPasswordModel model)
        {
            var result = new TokenStatusResponse();
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = string.Format(ErrorMessage.InvalidRequest)
                });
            }
            result = await iAuthentication.VerifyForgotPassword(mapper.Map<ResetPasswordRequest>(model));
            if (result.Status != HttpStatusCode.OK.ToString())
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = result.Message
                });
            }
            else
                return Ok(result);
        }
        #endregion
    }
}
