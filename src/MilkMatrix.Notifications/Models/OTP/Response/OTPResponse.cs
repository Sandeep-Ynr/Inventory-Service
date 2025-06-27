using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Notifications.Models.OTP.Response;

public class OTPResponse : StatusCode
{
    public bool IsOtpSent { get; set; } = false;

    public bool IsOtpSkiped { get; set; } = false;

    public bool OtpStatus { get; set; } = false;

    public int Otp { get; set; }
    public string? Status { get; set; }
}
