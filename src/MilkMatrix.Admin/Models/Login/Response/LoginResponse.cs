namespace MilkMatrix.Admin.Models.Login.Response;

public class LoginResponse
{
    public int Id { get; set; }
    public string? Email_Id { get; set; }
    public string? UserName { get; set; }
    public string? Hrms_Code { get; set; }
    public string? Mobile_No { get; set; }
    public int User_Type { get; set; }
    public string? SecKey { get; set; }
    public int Loginid { get; set; }
    public int Business_Id { get; set; }
    public int Reporting_Id { get; set; }
    public string? Image_Url { get; set; }
    public int FinancialYearId { get; set; }
    public string? FinancialYear { get; set; }
    public DateTime? FinancialYearFromDate { get; set; }
    public DateTime? FinancialYearToDate { get; set; }
    public string? Logo_Image_Path { get; set; }
    public string? Change_Password { get; set; }
    public string? Allbranch { get; set; }
    public string? Allow_Backup { get; set; }
    public string? Refresh_Token { get; set; }
    public DateTime? SecKeyExpiryOn { get; set; }
}
