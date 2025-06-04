using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Domain.Entities.Responses;
using static MilkMatrix.Api.Common.Constants.Constants;
using Asp.Versioning;
using MilkMatrix.Logging.Config;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Infrastructure.Models.Config;
using MilkMatrix.Infrastructure.Common.Logger.Interface;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models;
using AutoMapper;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Admin.Models.Login.Response;
using System.Security.Claims;

namespace MilkMatrix.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuth iAuthentication;
        private readonly AppConfig iConfiguration;
        private readonly IHttpContextAccessor ihttpContextAccessor;
        private readonly IMapper mapper;
        private ILogging logger;
        private IConfiguration configuration;

        [AllowAnonymous]
        [HttpPost]
        [Route("user-login/{isLogingWithOtp}")]
        public IActionResult AuthenticateUserLogin(bool isLogingWithOtp, LoginModel login)
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
            var loginResponse = iAuthentication.AuthenticateUserLogin(mapper.MapWithOptions<LoginRequest, LoginModel>(login
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
