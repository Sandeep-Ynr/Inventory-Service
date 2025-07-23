using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Report;

/// <summary>
/// Represents a log entry for auditing changes made to records in the system.
/// </summary>
public class AuditLogs
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user who made the changes.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who made the changes.
    /// </summary>
    [GlobalSearch]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the page where the changes were made.
    /// </summary>
    public int PageId { get; set; }

    /// <summary>
    /// Gets or sets the name of the page where the changes were made.
    /// </summary>
    [GlobalSearch]
    public string PageName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the action that was performed (e.g., insert, update, delete).
    /// </summary>
    [GlobalSearch]
    public string ColumnName { get; set; }

    /// <summary>
    /// Gets or sets the old value of the record before the changes were made.
    /// </summary>
    public string OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value of the record after the changes were made.
    /// </summary>
    public string NewValue { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the changes were made.
    /// </summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the record associated with the changes.
    /// </summary>
    [GlobalSearch]
    public string RecordNo { get; set; }

    /// <summary>
    /// Gets or sets the name of the table associated with the changes.
    /// </summary>
    [GlobalSearch]
    public string TableName { get; set; }
}
