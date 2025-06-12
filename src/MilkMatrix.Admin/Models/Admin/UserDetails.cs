namespace MilkMatrix.Admin.Models.Admin;

public class UserDetails
{
    public int UserId { get; set; }
    public string? EmailId { get; set; }
    public string? UserName { get; set; }
    public string? HrmsCode { get; set; }
    public string? MobileNo { get; set; }

    public string? MaskedMobile { get; set; }

    public string? MaskedEmail { get; set; }
    public int UserType { get; set; }
    public int ReportingId { get; set; }
    public string? ImageUrl { get; set; }

}
