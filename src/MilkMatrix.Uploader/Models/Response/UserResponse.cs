namespace MilkMatrix.Uploader.Models.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string? EmailId { get; set; }
    public string? MobileNumber { get; set; }
    public int BusinessId { get; set; }
    public int ReportingId { get; set; }
    public string? RoleId { get; set; }
    public string? UserName { get; set; }
    public int UserType { get; set; }
}
