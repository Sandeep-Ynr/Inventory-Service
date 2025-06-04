using MilkMatrix.Domain.Entities.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;

namespace MilkMatrix.Api.Common.Handlers
{
    //public class CustomTokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    //{
    //    private const string ApiKeyHeaderName = "Authorization";
    //    private const string CustomAuthenticationType = "custom";
    //    private readonly IAuth authClient;

    //    public CustomTokenHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    //        ILoggerFactory logger,
    //        UrlEncoder encoder,
    //        ISystemClock clock,
    //        IAuth authClient
    //        )
    //   : base(options, logger, encoder, clock)
    //    {
    //        this.authClient = authClient;
    //    }

    //    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    //    {
    //        // Set the user principal if authentication is successful
    //        if (!this.Context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentalApiKey))
    //        {
    //            this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
    //        }
    //        if (string.IsNullOrEmpty(potentalApiKey.ToString()))
    //        {
    //            this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
    //        }

    //        var authResponse = authClient.ValidateAppToken(potentalApiKey!);

    //        if (authResponse.Message != "")
    //        {
    //            this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
    //        }

    //        // Create claims
    //        var claims = new List<Claim>
    //        {
    //        new Claim(ClaimTypes.UserData, authResponse.UserId??string.Empty),
    //        new Claim(ClaimTypes.MobilePhone, authResponse.Mobile??string.Empty),
    //        new Claim(ClaimTypes.NameIdentifier, authResponse.BusinessId.ToString())
    //        };
    //        var identity = new ClaimsIdentity(claims, CustomAuthenticationType);

    //        var principal = new ClaimsPrincipal(identity);
    //        return AuthenticateResult.Success(new AuthenticationTicket(principal, CustomAuthenticationType));
    //    }
    //}

    public class CustomTokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string ApiKeyHeaderName = "token";
        private const string CustomAuthenticationType = "custom";
        private readonly IAuth authClient;

        public CustomTokenHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IAuth authClient
            )
       : base(options, logger, encoder)
        {
            this.authClient = authClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Set the user principal if authentication is successful
            if (!this.Context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentalApiKey))
            {
                this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
            }
            if (string.IsNullOrEmpty(potentalApiKey.ToString()))
            {
                this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
            }

            var authResponse = await authClient.ValidateAppToken(potentalApiKey!);

            if (authResponse.Message != "")
            {
                this.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return AuthenticateResult.Fail(new Exception(StatusCodeMessage.Unauthorized));
            }

            // Create claims
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.UserData, authResponse.UserId??string.Empty),
            new Claim(ClaimTypes.MobilePhone, authResponse.Mobile??string.Empty),
            new Claim(ClaimTypes.NameIdentifier, authResponse.BusinessId.ToString())
            };
            var identity = new ClaimsIdentity(claims, CustomAuthenticationType);

            var principal = new ClaimsPrincipal(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, CustomAuthenticationType));
        }
    }
}
