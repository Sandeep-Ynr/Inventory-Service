using MilkMatrix.Domain.Entities.Common;

namespace MilkMatrix.Admin.Business.Auth.Contracts
{
    public interface ITokenProcess
    {
        TokenEntity GenerateToken(string hostName, string userId, string mobile, int? businessId);
    }
}
