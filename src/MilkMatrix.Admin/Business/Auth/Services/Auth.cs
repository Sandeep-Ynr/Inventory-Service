using System.Net;
using Microsoft.Extensions.Configuration;
using MilkMatrix.Admin.Business.Auth.Contracts;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models.Login.Response;
using MilkMatrix.Domain.Entities.Common;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Infrastructure.Common.Logger.Interface;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Infrastructure.Contracts.Repositories;
using MilkMatrix.Infrastructure.Models.Config;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Auth.Services;

public class Auth : IAuth
{
    private readonly ITokenProcess tokenProcess;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IConfiguration configuration;

    private readonly ILogging logger;

    public Auth(ITokenProcess tokenProcess, IRepositoryFactory repositoryFactory, IConfiguration configuration, ILogging logging)
    {
        this.tokenProcess = tokenProcess;
        this.repositoryFactory = repositoryFactory;
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(Auth));
        this.logger = logging.ForContext("ServiceName", nameof(Auth));
    }

    public async Task<LoginMasterResponse> AuthenticateUserLogin(LoginRequest login)
    {
        LoginMasterResponse finalResult = new();
        LoginResponse lResponse = new();
        bool isAppLogin = false;
        try
        {
            login.TokenEntity = tokenProcess.GenerateToken(login.HostName, login.UserId, login.Mobile, login.BusinessId);
            string firebase_token = string.Empty;
            int loginId = 0;
            if (!login.IsLoginWithOtp)
            {
                login.Password = login.Password.EncodeSHA512();
            }
            var requestParam = new Dictionary<string, object>() { { "Emailid", login.UserId }
                ,{ "Password", login.Password },
                { "Login_Device_Mac", login.LoginDevice },
                { "Login_Public_Ip", login.PublicIP },
                { "Login_Private_Ip", login.PrivateIP },
                { "Login_Browser", login.BrowserName },
                { "SecKey", login.TokenEntity.token },
                { "RefreshToken", login.TokenEntity.RefreshToken },
                { "IsLoginWithOTP", login.IsLoginWithOtp },
                { "Otp", login.Otp },
                { "Mobile", login.Mobile },
                { "Latitude", login.Latitude },
                { "Longitude", login.Longitude },
                { "BusinessId", login.BusinessId }
            };
            var repo = repositoryFactory
                        .ConnectDapper<UserLoginResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<UserLoginResponse>(AuthSpName.UserLogin, requestParam, null);

            loginId = data.FirstOrDefault(data => data.Result.Equals("SUCCESS", StringComparison.InvariantCultureIgnoreCase))?.ID ?? 0;

            finalResult.Message = data?.FirstOrDefault()?.Result;
            if (loginId > 0)
            {
                var repoLogin = repositoryFactory
                       .ConnectDapper<LoginResponse>(DbConstants.Main);
                lResponse = (await repoLogin.QueryAsync<LoginResponse>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", loginId } }, null))!.SingleOrDefault()!;
                finalResult.Data = new TokenResponse
                {
                    AccessToken = lResponse.SecKey!,
                    ExpiresIn = lResponse.SecKeyExpiryOn,
                    RefreshToken = lResponse.RefreshToken!
                };
                logger.LogInfo("login successful");
            }

            #region HTTP Status
            if (finalResult.Message == "SUCCESS")
                finalResult.Status = HttpStatusCode.OK.ToString();
            else if (finalResult.Message == "Your current IP address is not authorized to access this resource.Please make sure you are connecting from an authorized IP address and try again.")
                finalResult.Status = HttpStatusCode.Unauthorized.ToString();
            else if (finalResult.Message == "Authentication/Authorization Failed.")
                finalResult.Status = HttpStatusCode.Unauthorized.ToString();
            else if (finalResult.Message == "User name or password does not exist.")
                finalResult.Status = HttpStatusCode.Unauthorized.ToString();
            #endregion
        }
        catch (Exception ex)
        {
            finalResult.Message = "Internal Server Error.";
            finalResult.Status = HttpStatusCode.InternalServerError.ToString();
            logger.LogError("Some error occurred while signIn", ex);
        }
        return finalResult;
    }
    public async Task<TokenStatusResponse> ValidateAppToken(string token)
    {
        TokenStatusResponse Meta = new();
        Meta.Message = "Unauthorized";
        Meta.Status = HttpStatusCode.Unauthorized.ToString();
        try
        {
            TokenEntity tokenEntity = new TokenEntity(); string decryptVal = string.Empty;
            var encrypt_Key = configuration.GetSection("AppConfiguration:Base64EncryptKey").Value;
            var sysHostmane = configuration.GetSection("AppConfiguration:HostName").Value?.ToLower();
            decryptVal = encrypt_Key.DecryptString(token.Split('`')[0]);
            var decryptedValues = decryptVal.Split('|');
            tokenEntity.hostName = decryptVal.Split('|')[0];
            tokenEntity.userID = decryptVal.Split('|')[1];
            Meta.UserId = tokenEntity.userID;
            Meta.Mobile = tokenEntity.mobile;
            Meta.BusinessId = decryptedValues.Length > 2 ? Convert.ToInt32(decryptVal.Split('|')[3]) : null;

            if (sysHostmane.Contains(tokenEntity.hostName))//Validate HostName
            {
                var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
                var res = (await repo.QueryAsync<string>(AuthSpName.ValidateToken, new Dictionary<string, object> { { "Emailid", tokenEntity.userID }
                ,{"SecKey", token } }, null))?.FirstOrDefault();
                Meta.Message = res;
                Meta.Status = string.IsNullOrEmpty(res) ? HttpStatusCode.OK.ToString() : HttpStatusCode.Unauthorized.ToString();
            }
        }
        catch (Exception ex)
        {
            logger.LogError("ValidateAppToken failed", ex);
        }

        var repoFact = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
        var response = (await repoFact.QueryAsync<int>(AuthSpName.GetUserId, new Dictionary<string, object> { { "Id", Meta.UserId } }, null))?.FirstOrDefault();

        Meta.UserId = response?.ToString();
        return Meta;
    }

    public async Task<TokenStatusResponse> ValidateRefreshToken(RefreshTokenRequest request)
    {
        TokenStatusResponse Meta = new();
        Meta.Message = "Unauthorized";
        Meta.Status = HttpStatusCode.Unauthorized.ToString();
        try
        {
            Meta.UserId = request.EmailId;
            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var res = (await repo.QueryAsync<string>(AuthSpName.ValidateRefreshToken, new Dictionary<string, object> { { "Emailid", request.EmailId }
                ,{"SecKey", request.Token },{"RefreshToken", request.RefreshToken } }, null))?.FirstOrDefault();
            Meta.Message = res;
        }
        catch (Exception ex)
        {
            logger.LogError("ValidateRefreshToken failed", ex);

        }
        return Meta;
    }

    public async Task<TokenStatusResponse> UpdateAccessToken(RefreshTokenRequest request)
    {
        TokenStatusResponse Meta = new();
        Meta.Message = "Unauthorized";
        Meta.Status = HttpStatusCode.Unauthorized.ToString();
        try
        {
            Meta.UserId = request.EmailId;
            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var res = (await repo.QueryAsync<string>(AuthSpName.ValidateRefreshToken, new Dictionary<string, object> { { "Emailid", request.EmailId }
                ,{"SecKey", request.Token },{"RefreshToken", request.RefreshToken } }, null))?.FirstOrDefault();
            Meta.Message = res;
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format("UpdateAccessToken failed for UserId : {0}", request.EmailId), ex);
        }
        return Meta;
    }

    public async Task<TokenStatusResponse> Userlogout(LogoutRequest logout)
    {
        TokenStatusResponse finalResult = new TokenStatusResponse();
        try
        {
            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var res = (await repo.QueryAsync<string>(AuthSpName.ValidateRefreshToken, new Dictionary<string, object> { { "LoginId", logout.LoginId }
                ,{"UserId", logout.UserId } }, null))?.FirstOrDefault();
            finalResult.Message = res;
            if (finalResult.Message == "Success")
            {
                finalResult.Status = HttpStatusCode.OK.ToString();
                logger.LogInfo(string.Format("LogOut successful for UserId : {0} LoginId : {1}", logout.UserId, logout.LoginId));
            }
            else
                finalResult.Status = HttpStatusCode.InternalServerError.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format("LogOut failed for UserId : {0}", logout.UserId), ex);
            finalResult.Status = HttpStatusCode.InternalServerError.ToString();
            finalResult.Message = HttpStatusCode.InternalServerError.ToString();
        }
        return finalResult;
    }

    public async Task<IEnumerable<LoginResponse>> GetUserDetailsAsync(string id, YesOrNo isEncryptionNeeded)
    {
        try
        {
            var repo = repositoryFactory
                       .ConnectDapper<LoginResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<LoginResponse>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", id } }, null);

            return data.Any() ? new List<LoginResponse>() { MaskAndEncryptUserResponse(data.FirstOrDefault()!, isEncryptionNeeded) } : Enumerable.Empty<LoginResponse>();
        }
        catch (Exception ex)
        {
            logger.LogError(Constants.ErrorMessage.GetError.FormatString(nameof(GetUserDetailsAsync)), ex);
            return Enumerable.Empty<LoginResponse>();
        }
    }

    private LoginResponse MaskAndEncryptUserResponse(LoginResponse response, YesOrNo isEncryptionNeeded)
    {
        var emailToEncrypt = response.EmailId!;
        var mobilToEncrypt = response.MobileNo!;
        response.MaskedMobile = response?.MobileNo?.MaskString();
        response.MaskedEmail = response?.EmailId?.MaskString();
        response.MobileNo = isEncryptionNeeded == YesOrNo.Yes ? mobilToEncrypt?.EncryptString(emailToEncrypt) : emailToEncrypt;
        response.EmailId = isEncryptionNeeded == YesOrNo.Yes ? mobilToEncrypt?.EncryptString(mobilToEncrypt) : mobilToEncrypt;
        return response;
    }
}
