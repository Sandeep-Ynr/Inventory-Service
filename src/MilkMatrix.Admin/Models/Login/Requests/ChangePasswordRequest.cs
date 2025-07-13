namespace MilkMatrix.Admin.Models.Login.Requests;

/// <summary>
/// Represents a request to change a user's password.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Gets or sets the user identifier for whom the password is being changed.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    public string OldPassword { get; set; } = string.Empty;


    public int LoggedInUser { get; set; }
}
