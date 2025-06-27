using Microsoft.Extensions.Configuration;
using MilkMatrix.Admin.Business.Auth.Contracts;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Infrastructure.Common.Utils;

namespace MilkMatrix.Admin.Business.Auth.Services;

/// <summary>
/// Provides functionality for generating authentication tokens for users.
/// </summary>
/// <remarks>
/// This class implements <see cref="ITokenProcess"/> and is responsible for creating secure tokens and refresh tokens
/// based on user and device information. It uses configuration values for encryption keys and utility methods for
/// string concatenation and encryption.
/// </remarks>
public class TokenProcess : ITokenProcess
{
    private readonly IConfiguration iConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenProcess"/> class.
    /// </summary>
    /// <param name="iConfiguration">The application configuration used to retrieve encryption keys.</param>
    public TokenProcess(IConfiguration iConfiguration)
    {
        this.iConfiguration = iConfiguration;
    }

    /// <summary>
    /// Generates a new <see cref="TokenEntity"/> for the specified user and device information.
    /// </summary>
    /// <param name="hostName">The host name from which the request originates.</param>
    /// <param name="userId">The unique identifier of the user. If null or empty, <paramref name="mobile"/> is used.</param>
    /// <param name="mobile">The mobile number of the user. Used if <paramref name="userId"/> is null or empty.</param>
    /// <param name="businessId">The business identifier associated with the user.</param>
    /// <returns>
    /// A <see cref="TokenEntity"/> containing a generated token and refresh token.
    /// </returns>
    /// <remarks>
    /// The method concatenates the host name, user key, and business ID, encrypts the result to form the token,
    /// and hashes it to form the refresh token. The encryption key is retrieved from configuration.
    /// </remarks>
    public TokenEntity GenerateToken(string hostName, string userId, string mobile, int? businessId)
    {
        string userKey = string.Empty;
        if (userId != null && userId != "")
            userKey = userId;
        else if (mobile != null && mobile != "")
            userKey = mobile;
        var encrypt_Key = iConfiguration.GetSection("AppConfiguration:Base64EncryptKey").Value!;
        string token = encrypt_Key.EncryptString(hostName.GetConcatenatedString(userKey, businessId)) + "`" + Guid.NewGuid().ToString();
        string refreshToken = hostName.GetConcatenatedString(userKey, businessId).EncodeSHA512();
        TokenEntity tEntity = new TokenEntity
        {
            token = token,
            RefreshToken = refreshToken
        };
        return tEntity;
    }
}
