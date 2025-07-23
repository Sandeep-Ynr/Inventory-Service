using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Report;

/// <summary>
/// Represents a log entry for events occurring in the system, such as user actions on pages.
/// </summary>
public class EventLogs
{
    /// <summary>
    /// Gets or sets the unique identifier for the event log entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user who performed the action associated with this event log entry.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who performed the action associated with this event log entry.
    /// </summary>
    [GlobalSearch]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the page where the action occurred.
    /// </summary>
    public int PageId { get; set; }

    /// <summary>
    /// Gets or sets the name of the page where the action occurred.
    /// </summary>
    [GlobalSearch]
    public string PageName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the action that was performed (e.g., insert, update, delete).
    /// </summary>
    public string ActionId { get; set; }

    /// <summary>
    /// Gets or sets the name of the action that was performed (e.g., "Insert", "Update", "Delete").
    /// </summary>
    [GlobalSearch]
    public string ActionName { get; set; }

    /// <summary>
    /// Gets or sets the document number associated with the action, if applicable.
    /// </summary>
    [GlobalSearch]
    public string ActionDocNo { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the action was performed.
    /// </summary>
    public DateTime ActionDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the business associated with this event log entry.
    /// </summary>
    public int BusinessId { get; set; }

    /// <summary>
    /// Gets or sets the name of the business associated with this event log entry.
    /// </summary>
    [GlobalSearch]
    public string BusinessName { get; set; }
}
