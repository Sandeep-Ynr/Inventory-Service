namespace MilkMatrix.Notifications.Models.Queries;

public static partial class ConfigurationSettings
{
    public static class NotificationSettings
    {
        public const string GetMailDetails = "usp_MailData";
        public static string GetSmsMerchantDetails = "usp_getSMS_Merchant_Details";
        public static readonly string GetSmsConfiguration = "usp_SMS_Configuration";
        public static readonly string SendOtpToUser = "usp_Send_Otp_To_User_InsUpd";
        public static readonly string GetServiceConfigurations = "usp_ServiceConfiguration";
    }

    public static class UserSpName
    {
        public const string GetUserId = "usp_Get_User_Id";
    }
}
