using System.Net;
using System.Security.Claims;
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
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Controllers.v1
{
    /// <summary>
    /// Controller for handling user login and authentication operations.
    /// This controller provides endpoints for user login, logout, password management, and token refresh operations.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="iAuthentication"></param>
        /// <param name="ihttpContextAccessor"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public LoginController(IAuth iAuthentication, IHttpContextAccessor ihttpContextAccessor, IMapper mapper, ILogging logger)
        {
            this.iAuthentication = iAuthentication;
            this.ihttpContextAccessor = ihttpContextAccessor;
            this.mapper = mapper;
            this.logger = logger.ForContext("ServiceName", nameof(LoginController));
        }

        /// <summary>
        /// Authenticates a user login request.
        /// </summary>
        /// <param name="isLogingWithOtp"></param>
        /// <param name="login"></param>
        /// <returns></returns>
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
            if (!isLogingWithOtp && (string.IsNullOrEmpty(login.UserId) || string.IsNullOrEmpty(login.Password)))
            {
                return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.BadRequest, ErrorMessage = string.Format(ErrorMessage.BadRequest, "UserId", "Password") });
            }
            if (isLogingWithOtp && (string.IsNullOrEmpty(login.Mobile) && string.IsNullOrEmpty(login.UserId)))
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
            if (loginResponse.Status != HttpStatusCode.OK.ToString())
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = loginResponse.Message
                });
            }
            else
                return Ok(loginResponse);
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("logged-in-details")]
        public async Task<ActionResult> GetUserDetails()
        {
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var response = await iAuthentication.GetUserDetailsAsync(UserId);

            return response != null && response.Any()
                ? Ok(response)
                : NotFound();
        }

        /// <summary>
        /// Refreshes the access token for the currently authenticated user using a valid refresh token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                    finalResponse.Data = (await iAuthentication.GetTokenResponseFromLoggedInUser(userData!.FirstOrDefault()!.UserId)).Item1;
                    finalResponse.Message = meta.Message;
                    return Ok(finalResponse);
                }
            }
            return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.Unauthorized, ErrorMessage = ErrorMessage.UnAuthorized });
        }

        /// <summary>
        /// Handles the forgot password request for a user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
            if (result.Status != ((int)HttpStatusCode.OK).ToString())
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

        /// <summary>
        /// Verifies the forgot password request for a user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Changes the password for the currently authenticated user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> UserChangePassword(ChangePasswordModel model)
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            var result = await iAuthentication.ChangePassword(mapper.MapWithOptions<ChangePasswordRequest, ChangePasswordModel>(model, new Dictionary<string, object> {
            { Constants.AutoMapper.LoginId ,Convert.ToInt32(userId)}
                }));
            if (result == null || result.Status != HttpStatusCode.OK.ToString())
            {
                return NotFound("No record found");
            }
            else
                return Ok(result);
        }


        /// <summary>
        /// Logs out the currently authenticated user and invalidates their session.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(LogoutModel request)
        {
            var userId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
            if (string.IsNullOrEmpty(userId) || request.UserId != Convert.ToInt32(userId))
            {
                return BadRequest(new ErrorResponse { StatusCode = (int)HttpStatusCode.Unauthorized, ErrorMessage = ErrorMessage.UnAuthorized });
            }
            var result = await iAuthentication.Userlogout(mapper.Map<LogoutRequest>(request));
            if (result == null || result.Status != HttpStatusCode.OK.ToString())
            {
                return NotFound("No record found");
            }
            else
                return Ok(result);
        }
    }
}
