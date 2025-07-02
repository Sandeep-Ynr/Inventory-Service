namespace MilkMatrix.Admin.Models.Admin.Requests.User;

public class UserProfileUpdate
{
    public int UserId { get; set; }
    public string? UserName { get; set; }

    public string? Mobile { get; set; }
    public int? ImageId { get; set; } = 0;

    public int ModifyBy { get; set; }
}
