namespace MilkMatrix.Admin.Models.Admin.Responses.User;

public class Users
{
    public int UserId { get; set; }
    public string? EmailId { get; set; }
    public string? UserName { get; set; }
    public string? HrmsCode { get; set; }
    public string? MobileNo { get; set; }

    public int UserType { get; set; }
    public int ReportingId { get; set; }
    public string? ImageUrl { get; set; }

    public string? BusinessId { get; set; }

    public string? ChangePassword { get; set; }

    public string? Allbranch { get; set; }

}
