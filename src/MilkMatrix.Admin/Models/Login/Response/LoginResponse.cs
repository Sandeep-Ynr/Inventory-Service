namespace MilkMatrix.Admin.Models.Login.Response;

public class LoginResponse
{
    public int UserId { get; set; }
    public string? EmailId { get; set; }

    public string? RoleId { get; set; }

    public string? BusinessId { get; set; }
    public string? UserName { get; set; }
    public string? HrmsCode { get; set; }
    public string? MobileNo { get; set; }

    public string? MaskedMobile { get; set; }

    public string? MaskedEmail { get; set; }
    public int UserType { get; set; }
    public string? SecKey { get; set; }
    public int Loginid { get; set; }
    public int ReportingId { get; set; }
    public string? ImageUrl { get; set; }
    public int FinancialYearId { get; set; }
    public string? FinancialYear { get; set; }
    public DateTime? FinancialYearFromDate { get; set; }
    public DateTime? FinancialYearToDate { get; set; }
    public string? LogoImagePath { get; set; }
    public string? ChangePassword { get; set; }
    public string? Allbranch { get; set; }
    public string? AllowBackup { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? SecKeyExpiryOn { get; set; }
}
