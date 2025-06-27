using MilkMatrix.Notifications.Models.Enums;

namespace MilkMatrix.Notifications.Models.OTP.Request;

public class OTPRequest
{
    public string MobileNumber { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;

    public OTPType OTPType { get; set; } = OTPType.Sms;

    public TemplateType TemplateType { get; set; }

    public string? Content { get; set; }
}
