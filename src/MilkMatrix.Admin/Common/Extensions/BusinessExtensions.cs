using MilkMatrix.Core.Entities.Response.Business;
using MilkMatrix.Core.Extensions;

namespace MilkMatrix.Admin.Common.Extensions;

internal static class BusinessExtensions
{
    internal static IEnumerable<BusinessDetails> WithFullPath(
        this IEnumerable<BusinessDetails> businessList,
        string domain)
    {
        foreach (var item in businessList)
        {
            item.LogoImagePath = item.LogoImagePath?.GetImagePath(domain);
        }
        return businessList;
    }
}
