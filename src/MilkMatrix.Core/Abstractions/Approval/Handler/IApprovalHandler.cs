using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Core.Abstractions.Approval.Handler;

/// <summary>
/// Defines the contract for approval handler operations in the application.
/// </summary>
public interface IApprovalHandler
{
    /// <summary>
    /// Attempts to approve a request based on the provided parameters.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<bool> ApproveAsync(Dictionary<string, object> parameters);

    /// <summary>
    /// Checks the conditions for approval based on the provided parameters.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<bool> CheckConditionsAsync(Dictionary<string, object> parameters);

    /// <summary>
    /// Gets the key that identifies the handler in the factory mapping system.
    /// </summary>
    FactoryMapping HandlerKey { get; }

}
