using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models.Login.Response;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Admin.Business.Auth.Contracts.Service;

public interface IAuth
{
    Task<LoginMasterResponse> AuthenticateUserLogin(LoginRequest login);
    Task<TokenStatusResponse> Userlogout(LogoutRequest logout);
    Task<TokenStatusResponse> ValidateAppToken(string token);
    Task<TokenStatusResponse> ValidateRefreshToken(RefreshTokenRequest request);
    Task<TokenStatusResponse> UpdateAccessToken(RefreshTokenRequest request);

    Task<IEnumerable<UserDetails>> GetUserDetailsAsync(string id);

    Task<TokenResponse> GetTokenResponseFromLoggedInUser(int loginId);

    Task<TokenStatusResponse> ForgotPassword(ForgotPasswordRequest request);

    Task<TokenStatusResponse> VerifyForgotPassword(ResetPasswordRequest model);
}
