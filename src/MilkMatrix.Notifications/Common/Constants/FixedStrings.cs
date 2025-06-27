namespace MilkMatrix.Notifications.Common.Constants;

public static partial class FixedStrings
{
    public const int SystemUser = 1;

    public const string Separator = ",";
    public const string UserAgent = "User-Agent";

    public const string Authorization = "Authorization";

    public const string Mobile = "mobileno";

    public const string SmsBody = "msgbody";

    public const string SenderId = "senderID";

    public const string DefaultDateTimeFormat = "yyyy-mm-dd hh:mm:ss";

    public const int OtpLength = 6;

    public const int RandomNumberLength = 6;

    public const int DefaultVerificationCode = 123456;

    public const string BlankValue = "";

    public const string Offset = "offset";

    public const string PageSize = "pageSize";

    public const string UserKeySplitter = "|";

    public const string GuidKeySplitter = "`";

    public const string Id = "Id";

    public const string DocNo = "DocNo";

    public const string Token = "SecKey";

    public static class SuccessMessage
    {
        public const string Message = "User successfully Created/Updated!!";
        public const string ResetPasswordSuccessMessage = "A verification code has been sent to your email address  {0}";
        public const string VerifyPasswordSuccessMessage = "Password has been successfully updated.";
        public const string OTPSuccess = "OTP has been sent to your mobile/email address.Please enter the same";
        public const string Success = "Success";
        public const string RecordInserted = "Record has been inserterd successfully.";
        public const string RecordUpdated = "Record has been updated successfully.";
        public const string RecordDeleted = "Record has been deleted successfully.";
    }

    public static class ErrorMessage
    {
        public const string Failed = "Failed";
        public const string DeviceTokenNotMatched = "Given Token is not match with existing token";
        public const string MobileNumberNotExists = "Mobile number not exist";
    }
}

