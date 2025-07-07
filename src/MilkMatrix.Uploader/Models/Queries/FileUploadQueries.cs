namespace MilkMatrix.Uploader.Models.Queries;

public sealed record FileUploadQueries
{
    public const string UploadFileRecords = "usp_File_Upload";

    public const string GetUserIdWithEmailId = "usp_Get_User_With_Email_id";

    public const string GetUploadFile = "usp_Get_UploadFile";
}
