using MilkMatrix.Notifications.Models.Enums;

namespace MilkMatrix.Notifications.Models.OTP.Request;

public class SystemEmailRequest
{
    public string EmailId { get; set; } = string.Empty;

    public TemplateType TemplateType { get; set; }

    public string Content { get; set; }
}
