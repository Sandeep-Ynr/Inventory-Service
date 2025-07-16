using System.Net;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Auth.Contracts;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models.Login.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Notification;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Dtos;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Admin.Models.Constants;
using static MilkMatrix.Core.Entities.Common.Constants;

namespace MilkMatrix.Admin.Business.Auth.Services;

public class Auth : IAuth
{
    private readonly ITokenProcess tokenProcess;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly AppConfig appConfig;

    private readonly FileConfig fileConfig;


    private ILogging logger;

    private readonly INotificationService notificationService;
    public Auth(ITokenProcess tokenProcess, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IOptions<FileConfig> fileConfig, ILogging logging, INotificationService notificationService)
    {
        this.tokenProcess = tokenProcess;
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(AppConfig));
        this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(FileConfig));
        this.logger = logging.ForContext("ServiceName", nameof(Auth));
        this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService), "NotificationService cannot be null");
    }

    /// <inheritdoc />
    public async Task<LoginMasterResponse> AuthenticateUserLogin(LoginRequest login)
    {
        LoginMasterResponse finalResult = new();
        LoginResponse lResponse = new();
        try
        {
            login.TokenEntity = tokenProcess.GenerateToken(login.HostName, login.UserId, login.Mobile, login.BusinessId);
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

            int verificationCode = appConfig.AllowToCreateOTP
                                 ? DefaultRandomNoLength.GenerateRandomNumber()
                                 : 123456;
            if (loginId > 0)
            {
                var tokenData = await GetTokenResponseFromLoggedInUser(loginId);
                finalResult.Data = tokenData.Item1;
                lResponse = tokenData.Item2;
                if (!string.IsNullOrWhiteSpace(finalResult.Data.IsMFA) && finalResult.Data.IsMFA == YesNoString.Yes && !login.IsLoginWithOtp)
                {
                    var otpResponse = await notificationService.SendAsync<NotificationRequest, NotificationResponse>(
                        new NotificationRequest
                        {
                            EmailId = lResponse.EmailId,
                            MobileNumber = lResponse.MobileNo,
                            TemplateType = NotificationTemplateType.Otp,
                            OTPType = NotificationType.Both,
                            Content = verificationCode.ToString()
                        });
                    if (otpResponse == null)
                    {
                        finalResult.Message = HttpStatusCode.InternalServerError.ToString();
                        finalResult.Status = HttpStatusCode.InternalServerError.ToString();
                    }

                    if (otpResponse.Code == (int)HttpStatusCode.OK)
                    {
                        finalResult.Message = string.Format(SuccessMessage.OTPSuccess);
                        finalResult.Status = otpResponse.Code.ToString();
                    }
                    else
                    {
                        finalResult.Message = otpResponse.Message ?? HttpStatusCode.InternalServerError.ToString();
                        finalResult.Status = otpResponse.Status ?? HttpStatusCode.InternalServerError.ToString();
                    }
                }

                logger.LogInfo("login successful");
            }

            #region HTTP Status
            if (finalResult.Message == "SUCCESS")
                finalResult.Status = HttpStatusCode.OK.ToString();
            else if (finalResult.Message == SuccessMessage.OTPSuccess)
            {
                finalResult.Data = new TokenResponse();
                finalResult.Status = HttpStatusCode.OK.ToString();
            }
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

    /// <inheritdoc />
    public async Task<(TokenResponse, LoginResponse)> GetTokenResponseFromLoggedInUser(int loginId)
    {
        var repoLogin = repositoryFactory
                               .ConnectDapper<LoginResponse>(DbConstants.Main);
        var lResponse = (await repoLogin.QueryAsync<LoginResponse>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", loginId } }, null))!.SingleOrDefault()!;
        var data = new TokenResponse
        {
            AccessToken = lResponse.SecKey!,
            ExpiresIn = lResponse.SecKeyExpiryOn,
            IsMFA = lResponse.IsMFA,
            RefreshToken = lResponse.RefreshToken!
        };
        return (data, lResponse);
    }

    /// <inheritdoc />
    public async Task<TokenStatusResponse> ValidateAppToken(string token)
    {
        TokenStatusResponse Meta = new();
        Meta.Message = "Unauthorized";
        Meta.Status = HttpStatusCode.Unauthorized.ToString();
        try
        {
            TokenEntity tokenEntity = new TokenEntity(); string decryptVal = string.Empty;
            var encrypt_Key = appConfig.Base64EncryptKey;
            var sysHostmane = appConfig.HostName.ToLower();
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IEnumerable<UserDetails>> GetUserDetailsAsync(string id)
    {
        try
        {
            var repo = repositoryFactory
                       .ConnectDapper<UserDetails>(DbConstants.Main);
            var data = await repo.QueryAsync<UserDetails>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", id } }, null);

            return data.Any() ? data.WithFullPath(fileConfig.UploadFileHost) : Enumerable.Empty<UserDetails>();
        }
        catch (Exception ex)
        {
            logger.LogError(Constants.ErrorMessage.GetError.FormatString(nameof(GetUserDetailsAsync)), ex);
            return Enumerable.Empty<UserDetails>();
        }
    }

    /// <inheritdoc />
    public async Task<TokenStatusResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        var result = new TokenStatusResponse();

        try
        {
            int verificationCode = appConfig.AllowToSendMail
                ? DefaultRandomNoLength.GenerateRandomNumber()
                : 123456;

            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var queryParams = new Dictionary<string, object>
        {
            { "ActionType", 1 },
            { "EmailId", request.EmailId },
            { "Otp", verificationCode }
        };

            var dbResult = (await repo.QueryAsync<string>(AuthSpName.PasswordChangeRequest, queryParams, null)).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(dbResult))
            {
                logger.LogWarning("ForgotPassword: No response from PasswordChangeRequest SP");
                result.Message = HttpStatusCode.InternalServerError.ToString();
                result.Status = HttpStatusCode.InternalServerError.ToString();
                return result;
            }

            if (!dbResult.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("ForgotPassword: No record found");
                result.Message = ErrorMessage.NoRecordFound;
                result.Status = HttpStatusCode.NotFound.ToString();
                return result;
            }

            // Send notification
            var notificationRequest = new NotificationRequest
            {
                EmailId = request.EmailId,
                TemplateType = NotificationTemplateType.ForgotPassword,
                OTPType = NotificationType.SystemEmail,
                Content = verificationCode.ToString()
            };

            var notificationResponse = await notificationService.SendAsync<NotificationRequest, NotificationResponse>(notificationRequest);

            if (notificationResponse == null)
            {
                logger.LogError($"ForgotPassword: Notification service returned null for EmailId");
                result.Message = HttpStatusCode.InternalServerError.ToString();
                result.Status = HttpStatusCode.InternalServerError.ToString();
                return result;
            }

            if (notificationResponse.Code == (int)HttpStatusCode.OK)
            {
                result.Message = string.Format(SuccessMessage.ResetPasswordSuccessMessage, request.EmailId);
                result.Status = notificationResponse.Code.ToString();
            }
            else
            {
                result.Message = notificationResponse.Message ?? HttpStatusCode.InternalServerError.ToString();
                result.Status = notificationResponse.Status ?? HttpStatusCode.InternalServerError.ToString();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format("ForgotPassword: Exception: {0}", ex));
            result.Message = HttpStatusCode.InternalServerError.ToString();
            result.Status = HttpStatusCode.InternalServerError.ToString();
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<TokenStatusResponse> VerifyForgotPassword(ResetPasswordRequest model)
    {
        var result = new TokenStatusResponse();

        try
        {
            // Hash the password
            var encryptedPassword = model.Password.EncodeSHA512();

            // Prepare parameters
            var parameters = new Dictionary<string, object>
        {
            { "EmailId", model.EmailId },
            { "EncryptPassword", encryptedPassword },
            { "Otp", model.SecurityCode }
        };

            // Execute query
            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var dbResult = (await repo.QueryAsync<string>(AuthSpName.VerifyPasswordChange, parameters, null)).FirstOrDefault();

            // Check result
            if (string.Equals(dbResult, "password has been successfully updated", StringComparison.OrdinalIgnoreCase))
            {
                result.Message = dbResult;
                result.Status = HttpStatusCode.OK.ToString();
                logger.LogInfo($"VerifyForgotPassword: Password updated");
                return result;
            }

            // Handle failure
            result.Message = dbResult ?? StatusCodeMessage.InternalServerError;
            result.Status = HttpStatusCode.InternalServerError.ToString();
            logger.LogWarning($"VerifyForgotPassword: Failed, Message: {dbResult}");
        }
        catch (Exception ex)
        {
            result.Message = StatusCodeMessage.InternalServerError;
            result.Status = HttpStatusCode.InternalServerError.ToString();
            logger.LogError($"VerifyForgotPassword: Exception", ex);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<TokenStatusResponse> ChangePassword(ChangePasswordRequest model)
    {
        var result = new TokenStatusResponse();

        try
        {


            // Hash the password if provided
            var hashedPassword = !string.IsNullOrEmpty(model.NewPassword)
                ? model.NewPassword.EncodeSHA512()
                : string.Empty;

            var userId = string.IsNullOrEmpty(model.UserId) ? model.LoggedInUser.ToString() : model.UserId;
            if (!string.IsNullOrEmpty(model.UserId))
            {
                var repoFact = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
                userId = (await repoFact.QueryAsync<int>(AuthSpName.GetUserId, new Dictionary<string, object> { { "Id", model.UserId } }, null))?.FirstOrDefault().ToString();
            }

            YesOrNo? isOldPasswordMatched = YesOrNo.No;
            if (!string.IsNullOrEmpty(model.OldPassword))
            {
                var repoFact = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
                isOldPasswordMatched = (await repoFact.QueryAsync<YesOrNo>(AuthSpName.VerifyOldPassword,
                    new Dictionary<string, object> { { "Id", model.UserId }, { "oldPassword", model.OldPassword.EncodeSHA512() } }, null))?.FirstOrDefault();
            }

            if (isOldPasswordMatched.HasValue && isOldPasswordMatched.Value == YesOrNo.No)
            {
                result.Message = StatusCodeMessage.OldPasswordNotMatching;
                result.Status = HttpStatusCode.InternalServerError.ToString();
                logger.LogError($"UserChangePassword: Exception for UserId: {model.UserId}", new Exception(result.Message));
                return result;
            }

            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                result.Message = "Password cannot be empty.";
                result.Status = HttpStatusCode.BadRequest.ToString();
                logger.LogWarning($"UserChangePassword: Password is empty for UserId: {model.UserId}");
                return result;
            }

            var queryParams = new Dictionary<string, object>
                 {
                    { "Id", userId },
                    { "Password", hashedPassword },
                    { "EncryptPassword", hashedPassword }
                 };

            var repo = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
            var dbResult = (await repo.QueryAsync<string>(AuthSpName.UserChangePassword, queryParams, null)).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(dbResult) && dbResult.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                result.Message = dbResult;
                result.Status = HttpStatusCode.OK.ToString();
                logger.LogInfo($"UserChangePassword: Password changed successfully for UserId: {model.UserId}");
                return result;
            }

            if (string.IsNullOrWhiteSpace(dbResult))
            {
                logger.LogWarning($"UserChangePassword: No response from UserChangePassword SP for UserId: {model.UserId}");
                result.Message = StatusCodeMessage.InternalServerError;
                result.Status = HttpStatusCode.InternalServerError.ToString();
                return result;
            }

            result.Message = dbResult;
            result.Status = HttpStatusCode.InternalServerError.ToString();
            logger.LogWarning($"UserChangePassword: Failed to change password for UserId: {model.UserId}, Message: {dbResult}");
        }
        catch (Exception ex)
        {
            result.Message = StatusCodeMessage.InternalServerError;
            result.Status = HttpStatusCode.InternalServerError.ToString();
            logger.LogError($"UserChangePassword: Exception for UserId: {model.UserId}", ex);
        }

        return result;
    }

    private UserDetails MaskAndEncryptUserResponse(UserDetails response)
    {
        response.MaskedMobile = response?.MobileNo?.MaskString();
        response.MaskedEmail = response?.EmailId?.MaskString();
        //response.MobileNo = !string.IsNullOrEmpty(response.MobileNo) ? appConfig.Base64EncryptKey?.EncryptString(response.MobileNo) : string.Empty;
        //response.EmailId = !string.IsNullOrEmpty(response.EmailId) ? appConfig.Base64EncryptKey?.EncryptString(response.EmailId) : string.Empty;
        return response;
    }

}
