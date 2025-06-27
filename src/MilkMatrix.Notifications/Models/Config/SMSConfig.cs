using MilkMatrix.Notifications.Models.Enums;

namespace MilkMatrix.Notifications.Models.Config;

public sealed record SMSConfig
{
    public const string SectionName = "SMSConfiguration";

    public OTPActionType ActionType { get; set; } = OTPActionType.Single;

    public OTPEnum OtpNeed { get; set; } = OTPEnum.Success;
}
