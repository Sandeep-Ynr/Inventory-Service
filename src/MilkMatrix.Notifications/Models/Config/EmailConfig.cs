using MilkMatrix.Notifications.Models.Enums;

namespace MilkMatrix.Notifications.Models.Config;

public class EmailConfig
{
    public const string SectionName = "EmailConfiguration";

    public OTPEnum OtpNeed { get; set; } = OTPEnum.Success;
}
