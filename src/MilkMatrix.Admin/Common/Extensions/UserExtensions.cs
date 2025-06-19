using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Infrastructure.Common.Utils;

namespace MilkMatrix.Admin.Common.Extensions;

public static class UserExtensions
{
    public static UserDetails MaskAndEncryptUserResponse(this UserDetails response, string key)
    {
        response.MaskedMobile = response?.MobileNo?.MaskString();
        response.MaskedEmail = response?.EmailId?.MaskString();
        response.MobileNo = !string.IsNullOrEmpty(response.MobileNo) ? key.EncryptString(response.MobileNo) : string.Empty;
        response.EmailId = !string.IsNullOrEmpty(response.EmailId) ? key.EncryptString(response.EmailId) : string.Empty;
        return response;
    }
}
