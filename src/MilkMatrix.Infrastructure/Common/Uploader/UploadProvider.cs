using AutoMapper;
using MilkMatrix.Core.Abstractions.Uploader;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Uploader.Contracts.Services;
using MilkMatrix.Uploader.Models.Request;
using MilkMatrix.Uploader.Models.Response;

namespace MilkMatrix.Infrastructure.Common.Uploader;

public class UploadProvider(IUploadService uploadService, IMapper mapper) : IFileUploader
{
    public Task<IEnumerable<T>> GetUserId<T>(string userId, string mobileNumber, int businessId = 0) =>
        uploadService.GetUserId<T>(userId, mobileNumber, businessId);

    public async Task<IEnumerable<TResponse>> SaveFileResponse<TResponse>(List<TResponse> fileResponse)
    {
        // Only support FileResponse or mapped types
        if (typeof(TResponse) == typeof(FileResponse))
        {
            return await uploadService.SaveFileResponse<TResponse>(fileResponse);
        }
        if (typeof(TResponse) == typeof(UploadResponse))
        {
            var fileReqList = mapper.Map<List<FileResponse>>(fileResponse);
            var fileRespList = await uploadService.SaveFileResponse(fileReqList);
            var mapped = mapper.Map<List<UploadResponse>>(fileRespList);
            return (IEnumerable<TResponse>)(object)mapped;
        }
        throw new NotSupportedException("Unsupported type combination for SaveFileResponse.");
    }

    public async Task<IEnumerable<TResponse>> UploadFile<TRequest, TResponse>(IEnumerable<TRequest> files, string UID, bool isCsv = false)
    {
        // Only support UploadRequest -> UploadResponse or FileRequest -> FileResponse
        if (typeof(TRequest) == typeof(FileRequest) && typeof(TResponse) == typeof(FileResponse))
        {
            return await uploadService.UploadFile<TRequest, TResponse>(files, UID, isCsv);
        }
        if (typeof(TRequest) == typeof(UploadRequest) && typeof(TResponse) == typeof(UploadResponse))
        {
            var fileRequests = mapper.Map<List<FileRequest>>(files);
            var fileResponses = await uploadService.UploadFile<FileRequest, FileResponse>(fileRequests, UID, isCsv);
            var mapped = mapper.Map<List<UploadResponse>>(fileResponses);
            return (IEnumerable<TResponse>)(object)mapped;
        }
        throw new NotSupportedException("Unsupported type combination for UploadFile.");
    }

    public async Task<IEnumerable<TResponse>> UploadFileWithName<TRequest, TResponse>(TRequest files, string UID, bool isCsv = false)
    {
        // Only support UploadRequest -> UploadResponse or FileRequest -> FileResponse
        if (typeof(TRequest) == typeof(FileRequest) && typeof(TResponse) == typeof(FileResponse))
        {
            return await uploadService.UploadFileWithName<TRequest, TResponse>(files, UID, isCsv);
        }
        if (typeof(TRequest) == typeof(UploadRequest) && typeof(TResponse) == typeof(UploadResponse))
        {
            var fileRequest = mapper.Map<FileRequest>(files);
            var fileResponses = await uploadService.UploadFileWithName<FileRequest, FileResponse>(fileRequest, UID, isCsv);
            var mapped = mapper.Map<List<UploadResponse>>(fileResponses);
            return (IEnumerable<TResponse>)(object)mapped;
        }
        throw new NotSupportedException("Unsupported type combination for UploadFileWithName.");
    }
}
