using MilkMatrix.Notifications.Models.Enums;

namespace MilkMatrix.Notifications.Models.OTP.Request;

public class EmailRequest
{
    public string EmailId { get; set; } = string.Empty;

    public TemplateType TemplateType { get; set; }

    public int? VerificationCode { get; set; }
}
