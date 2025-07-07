namespace MilkMatrix.Uploader.Common;

public static partial class Constants
{
    public static class ErrorMessage
    {
        public const string BadRequest = "Invalid input {0}, {1} are required parameters";
        public const string NotFound = "No records found for current user";
        public const string UnsupportedFileExtension = "File {0} has an unsupported format";
        public const string FileLengthError = "File {0} is too large";
        public const string NoFileError = "No files were uploaded";
        public const string FileUploadError = "Some error occurred during upload!!";
        public const string Error = "Some error occurred during creation please connect with administrator!!";
        public const string Exception = "Some error occurred in {0} method exception {1} :: stacktrace {2}";
        public const string MissingParameter = "Any of following required parameters not passed {0}";
        public const string GetError = "An error occurred in a GET for {0}";
        public const string NothingSelectedToUpload = "Please select atleast one file to upload";
        public const string MaxFileAllowedExceeded = "You have passed the maximum no of files allowed to upload please try to upload lesser no. of files";
    }
}
