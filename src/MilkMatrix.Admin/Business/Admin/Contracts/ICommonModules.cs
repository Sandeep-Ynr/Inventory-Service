using MilkMatrix.Admin.Models.Admin.Common;
using MilkMatrix.Admin.Models.Admin.Responses.Modules;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface ICommonModules
{
    Task<CommonUserDetails> GetCommonDetails(string userId, string mobileNumber);

    Task<ModuleResponse> GetModulesAsync(string userId, string mobileNumber);
}
