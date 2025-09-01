using MilkMatrix.Core.Attributes;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;

public class CommonStatusDetails : Audit
{
    /// <summary>
    /// Gets or sets the unique identifier for the status.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the status.
    /// </summary>
    [GlobalSearch]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the status, which can be used to categorize or group statuses.
    /// </summary>
    public string StatusType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this status is active.
    /// </summary>
    public bool ShowToUser { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this status is the default status.
    /// </summary>
    public bool ShowFollowUpDateTime { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent status, if applicable.
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Gets or sets the name of the parent status, if applicable.
    /// </summary>
    public string ParentStatusName { get; set; }

    /// <summary>
    /// Gets or sets the abbreviation for the status, which can be used for display purposes.
    /// </summary>
    public string Abbreviation { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the business associated with this status.
    /// </summary>
    public int BusinessId { get; set; }

    /// <summary>
    /// Gets or sets the name of the business associated with this status.
    /// </summary>
    [GlobalSearch]
    public string BusinessName { get; set; }

}
