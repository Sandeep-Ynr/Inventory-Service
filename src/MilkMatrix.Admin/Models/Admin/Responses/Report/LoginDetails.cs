using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Report;

/// <summary>
/// Represents the details of a user login session in the system.
/// </summary>
public class LoginDetails
{
    /// <summary>
    /// Gets or sets the unique identifier for the login session.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user associated with this login session.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user associated with this login session.
    /// </summary>
    [GlobalSearch]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the page where the login occurred.
    /// </summary>
    public DateTime LoginDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user logged out of the system.
    /// </summary>
    public DateTime LogOutDate { get; set; }
}
