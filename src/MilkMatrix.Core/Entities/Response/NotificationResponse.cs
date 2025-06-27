using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Core.Entities.Response;

public class NotificationResponse : StatusCode
{
    public bool IsOtpSent { get; set; } = false;

    public bool IsOtpSkiped { get; set; } = false;

    public bool OtpStatus { get; set; } = false;

    public int Otp { get; set; }
    public string? Status { get; set; }
}
