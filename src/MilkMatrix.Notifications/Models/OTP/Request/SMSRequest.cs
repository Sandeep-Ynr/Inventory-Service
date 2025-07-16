namespace MilkMatrix.Notifications.Models.OTP.Request;

public class SMSRequest
{
    public string MobileNumber { get; set; } = string.Empty;

    public int? VerificationCode { get; set; }
}
