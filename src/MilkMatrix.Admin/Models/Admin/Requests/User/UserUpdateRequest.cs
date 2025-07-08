namespace MilkMatrix.Admin.Models.Admin.Requests.User;

public class UserUpdateRequest
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? EmailId { get; set; }

    public string? HrmsCode { get; set; }

    public string? RoleId { get; set; }

    public string? BusinessId { get; set; }

    public string? ReportingId { get; set; }
    public int? UserType { get; set; }

    public int? ImageId { get; set; }
    public string? MobileNumber { get; set; }
    public int ModifyBy { get; set; }

    public bool? IsActive { get; set; }
}
