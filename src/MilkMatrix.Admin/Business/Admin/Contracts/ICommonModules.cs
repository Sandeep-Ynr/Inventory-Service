using MilkMatrix.Admin.Models.Admin.Common;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Responses.Modules;
using MilkMatrix.Core.Entities.Response.Business;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for common module operations in the application.
/// </summary>
public interface ICommonModules
{
    /// <summary>
    /// Retrieves common user details based on user ID and mobile number.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="mobileNumber"></param>
    /// <returns></returns>
    Task<CommonUserDetails> GetCommonDetails(string userId, string mobileNumber);

    /// <summary>
    /// Retrieves a list of modules available to the user based on their ID and mobile number.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="mobileNumber"></param>
    /// <returns></returns>
    Task<ModuleResponse> GetModulesAsync(string userId, string mobileNumber);

    /// <summary>
    /// Retrieves a list of active/inactive financial year available based their ID or all.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IEnumerable<FinancialYearDetails>> GetFinancialYearAsync(FinancialYearRequest request);
}
