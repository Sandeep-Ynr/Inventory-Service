using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Core.Entities.Common;

public class NotificationRequest
{
    public string MobileNumber { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;

    public NotificationType OTPType { get; set; } = NotificationType.Email;

    public NotificationTemplateType TemplateType { get; set; }

    public string? Content { get; set; }
}
