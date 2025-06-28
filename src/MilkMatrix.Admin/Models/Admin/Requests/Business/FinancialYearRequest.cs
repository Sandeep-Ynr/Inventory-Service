using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Business;

/// <summary>
/// Represents a model for managing financial years in the application.
/// </summary>
public class FinancialYearRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the financial year.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the action type for reading financial years.
    /// Allows filtering based on the action type, such as retrieving all financial years or based on financial id.
    /// </summary>
    public ReadActionType ActionType { get; set; } = ReadActionType.All;

    /// <summary>
    /// Gets or sets the status of the financial year. 
    /// </summary>
    public bool? IsActive { get; set; } = true;
}
