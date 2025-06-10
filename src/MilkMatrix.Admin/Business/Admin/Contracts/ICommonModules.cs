using MilkMatrix.Admin.Models.Admin;
using MilkMatrix.Admin.Models.Admin.Common;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface ICommonModules
{
    Task<CommonUserDetails> GetCommonDetails(string userId, string mobileNumber);

    Task<ModuleResponse> GetModulesAsync(string userId, string mobileNumber);
}
