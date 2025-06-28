using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Business;

/// <summary>
/// Represents a request model for business data operations in the application.
/// </summary>
public class BusinessDataRequest
{
    /// <summary>
    /// Gets or sets the action type for the business data request.
    /// </summary>
    public BusinessActionType ActionType { get; set; } = BusinessActionType.ReportingForms;
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int UserId { get; set; }
}
