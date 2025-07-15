using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Admin.Models.Login.Response;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Admin.Business.Auth.Contracts.Service;

/// <summary>
/// Provides authentication and user account management services.
/// </summary>
public interface IAuth
{
    /// <summary>
    /// Authenticates a user with the provided login credentials.
    /// </summary>
    /// <param name="login">The login request containing user credentials and device information.</param>
    /// <returns>A <see cref="LoginMasterResponse"/> containing authentication status and user token data.</returns>
    Task<LoginMasterResponse> AuthenticateUserLogin(LoginRequest login);

    /// <summary>
    /// Logs out the specified user.
    /// </summary>
    /// <param name="logout">The logout request containing user and session identifiers.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the result of the logout operation.</returns>
    Task<TokenStatusResponse> Userlogout(LogoutRequest logout);

    /// <summary>
    /// Validates the provided application access token.
    /// </summary>
    /// <param name="token">The access token to validate.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the validity of the token.</returns>
    Task<TokenStatusResponse> ValidateAppToken(string token);

    /// <summary>
    /// Validates the provided refresh token for a user.
    /// </summary>
    /// <param name="request">The refresh token request containing user and token information.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the validity of the refresh token.</returns>
    Task<TokenStatusResponse> ValidateRefreshToken(RefreshTokenRequest request);

    /// <summary>
    /// Updates the access token using a valid refresh token.
    /// </summary>
    /// <param name="request">The refresh token request containing user and token information.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the result of the update operation.</returns>
    Task<TokenStatusResponse> UpdateAccessToken(RefreshTokenRequest request);

    /// <summary>
    /// Retrieves user details for the specified user identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>An enumerable of <see cref="UserDetails"/> for the user.</returns>
    Task<IEnumerable<UserDetails>> GetUserDetailsAsync(string id);

    /// <summary>
    /// Retrieves the token response for a logged-in user.
    /// </summary>
    /// <param name="loginId">The login identifier.</param>
    /// <returns>A <see cref="TokenResponse"/> containing access and refresh tokens.</returns>
    Task<(TokenResponse, LoginResponse)> GetTokenResponseFromLoggedInUser(int loginId);

    /// <summary>
    /// Initiates the forgot password process for the specified user.
    /// </summary>
    /// <param name="request">The forgot password request containing user information.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the result of the operation.</returns>
    Task<TokenStatusResponse> ForgotPassword(ForgotPasswordRequest request);

    /// <summary>
    /// Verifies the OTP and updates the password for a user who has forgotten their password.
    /// </summary>
    /// <param name="model">The reset password request containing email, OTP, and new password.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the result of the verification and update.</returns>
    Task<TokenStatusResponse> VerifyForgotPassword(ResetPasswordRequest model);

    /// <summary>
    /// Changes the password for the specified user.
    /// </summary>
    /// <param name="model">The change password request containing user ID and new password.</param>
    /// <returns>A <see cref="TokenStatusResponse"/> indicating the result of the password change operation.</returns>
    Task<TokenStatusResponse> ChangePassword(ChangePasswordRequest model);
}
