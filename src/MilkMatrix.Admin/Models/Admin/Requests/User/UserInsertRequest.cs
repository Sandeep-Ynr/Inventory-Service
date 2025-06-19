namespace MilkMatrix.Admin.Models.Admin.Requests.User;

public class UserInsertRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;

    public string HrmsCode { get; set; } = string.Empty;

    public string RoleId { get; set; } = string.Empty;

    public string BusinessId { get; set; } = string.Empty;

    public string ReportingId { get; set; } = string.Empty;

    public int? UserType { get; set; }

    public int? ImageId { get; set; } 
    public string MobileNumber { get; set; } = string.Empty;

    public int CreatedBy { get; set; }
}
