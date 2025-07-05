namespace MilkMatrix.Core.Abstractions.Uploader;

public interface IFileUploader
{
    Task<IEnumerable<TResponse>> UploadFile<TRequest, TResponse>(IEnumerable<TRequest> files, string UID, bool isCsv = false);

    Task<IEnumerable<TResponse>> UploadFileWithName<TRequest, TResponse>(TRequest files, string UID, bool isCsv = false);
    Task<IEnumerable<TResponse>> SaveFileResponse<TResponse>(List<TResponse> fileResponse);

    Task<IEnumerable<T>> GetUserId<T>(string userId, string mobileNumber, int businessId = 0);
}
