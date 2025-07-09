using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.Utils;

namespace MilkMatrix.Admin.Common.Extensions;

internal static class UserExtensions
{
    internal static UserDetails MaskAndEncryptUserResponse(this UserDetails response, string key)
    {
        response.MaskedMobile = response?.MobileNo?.MaskString();
        response.MaskedEmail = response?.EmailId?.MaskString();
        response.MobileNo = !string.IsNullOrEmpty(response.MobileNo) ? key.EncryptString(response.MobileNo) : string.Empty;
        response.EmailId = !string.IsNullOrEmpty(response.EmailId) ? key.EncryptString(response.EmailId) : string.Empty;
        return response;
    }

    internal static IEnumerable<UserDetails> WithFullPath(
       this IEnumerable<UserDetails> list,
       string domain)
    {
        foreach (var item in list)
        {
            item.ImageUrl = item.ImageUrl?.GetImagePath(domain);
        }
        return list;
    }

    internal static IEnumerable<Users> WithFullPath(
       this IEnumerable<Users> list,
       string domain)
    {
        foreach (var item in list)
        {
            item.ImageUrl = item.ImageUrl?.GetImagePath(domain);
        }
        return list;
    }
}
