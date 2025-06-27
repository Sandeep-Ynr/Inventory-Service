namespace MilkMatrix.Core.Entities.Common;

public static partial class Constants
{
    public const int DefaultRandomNoLength = 6;
    public static class ErrorMessage
    {
        public const string Error = "Some error occurred during creation please connect with administrator!!";
        public const string FileUploadError = "Some error occurred during upload!!";
        public const string Exception = "Some error occurred in {0} method exception {1} :: stacktrace {2}";
        public const string MissingParameter = "Any of following required parameters not passed {0}";
        public const string UnsupportedFileExtension = "File {0} has an unsupported format";
        public const string FileLengthError = "File {0} is too large";
        public const string NoFileError = "No files were uploaded";
        public const string WrongVerificationCode = "You have entered a wrong verification code";
        public const string UserIdNotExists = "User doesn't exists";
        public const string UpdateError = "Some error occurred during updation please connect with administrator!!";
        public const string Failed = "Failed";
        public const string DeviceTokenNotMatched = "Given Token is not match with existing token";
        public const string MobileNumberNotExists = "Mobile number not exist";
        public const string GetError = "An error occurred in a GET for {0}";
        public const string PostError = "An error occurred in a Post request for {0}";
        public const string OtpTypeMessage = "Otp type not specified";
        public const string InvalidDetails = "Invalid email/mobile/otp/password.";
        public const string InvalidIp = "Your current IP address is not authorized to access this resource.Please make sure you are connecting from an authorized IP address and try again.";
        public const string UserNotExists = "User name or password does not exist";
        public const string NoRecordFound = "No record found";
        public const string NothingSelectedToUpload = "Please select atleast one file to upload";
        public const string MaxFileAllowedExceeded = "You have passed the maximum no of files allowed to upload please try to upload lesser no. of files";
        public const string RequiredParamMissing = "Required Parameter is missing please check your request";
        public const string InvalidModelState = "Required parameters not passed";
        public const string PanVerificationError = "Error while verifying pan No please check creds";
        public const string PanVerificationUserError = "Error during pan verification.";
        public const string RecordAlreadyExists = "Record already exists.";
        public const string RecordFailed = "Failure.";
    }

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
}
