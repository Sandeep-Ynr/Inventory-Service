using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.User;

public class Users
{
    public int UserId { get; set; }

    [GlobalSearch]
    public string? EmailId { get; set; }

    [GlobalSearch]
    public string? UserName { get; set; }
    public string? HrmsCode { get; set; }

    [GlobalSearch]
    public string? MobileNo { get; set; }

    public int UserType { get; set; }
    public int ReportingId { get; set; }
    public string? ImageUrl { get; set; }

    public string? BusinessId { get; set; }

    public string? ChangePassword { get; set; }

    public string? Allbranch { get; set; }

    public string ReportingToName { get; set; } = string.Empty;
    public string UserTypeName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

}
