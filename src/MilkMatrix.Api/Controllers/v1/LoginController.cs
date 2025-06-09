using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Infrastructure.Common.Logger.Interface;
using MilkMatrix.Infrastructure.Common.Utils;
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
            this.logger = logger.ForContext("ServiceName",nameof(LoginController));
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
            GetUserBrowserDetails(out var publicIpAddress, out var privateIpAddress, out var userAgent);
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
        public async Task<ActionResult> GetUserDetails([FromQuery] YesOrNo e = YesOrNo.Yes)
        {
            var UserId = ihttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

            var response = await iAuthentication.GetUserDetailsAsync(UserId, e);

            return response != null && response.Any()
                ? Ok(response)
                : NotFound();
        }

        private void GetUserBrowserDetails(out IPAddress? publicIpAddress, out IPAddress? privateIpAddress, out string? userAgent)
        {
            publicIpAddress = ihttpContextAccessor?.HttpContext?.Connection.RemoteIpAddress;
            privateIpAddress = ihttpContextAccessor?.HttpContext?.Connection.LocalIpAddress;
            userAgent = ihttpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString();
            GetIpAddress(ref publicIpAddress, ref privateIpAddress);
        }

        private static void GetIpAddress(ref IPAddress? publicIpAddress, ref IPAddress? privateIpAddress)
        {
            if (publicIpAddress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                publicIpAddress = Dns.GetHostEntry(publicIpAddress).AddressList
          .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            }
            if (privateIpAddress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                privateIpAddress = Dns.GetHostEntry(privateIpAddress).AddressList
          .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }
        }
    }
}
