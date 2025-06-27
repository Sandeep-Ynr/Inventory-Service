using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Login.Requests;

public class LoginRequest
{
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public string? Mobile { get; set; }
    public int Otp { get; set; }
    public string? BrowserName { get; set; }
    public string? PrivateIP { get; set; }
    public string? PublicIP { get; set; }
    public string? LoginDevice { get; set; }
    public TokenEntity? TokenEntity { get; set; }
    public string? HostName { get; set; }
    public int BusinessId { get; set; }
    public bool IsLoginWithOtp { get; set; }

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
