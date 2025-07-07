using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Uploader.Contracts.Services;
using MilkMatrix.Uploader.Helpers;
using MilkMatrix.Uploader.Models.Config;
using MilkMatrix.Uploader.Models.Queries;
using MilkMatrix.Uploader.Models.Request;
using MilkMatrix.Uploader.Models.Response;
using static MilkMatrix.Uploader.Common.Constants;

namespace MilkMatrix.Uploader.Implementation.Services;

public class UploadService : IUploadService
{
    private readonly ILogging logger;
    private readonly FileConfig fileConfig;
    private readonly IRepositoryFactory repositoryFactory;
    private readonly UploadConfig uploadConfig;

    public UploadService(
        ILogging logger,
        IOptions<FileConfig> fileConfig,
        IRepositoryFactory repositoryFactory,
        IOptions<UploadConfig> uploadConfig
    )
    {
        this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig));
        this.repositoryFactory = repositoryFactory;
        this.logger = logger.ForContext("ServiceName", nameof(UploadService));
        this.uploadConfig = uploadConfig.Value ?? throw new ArgumentNullException(nameof(UploadConfig));
    }

    public async Task<IEnumerable<TResponse>> UploadFile<TRequest, TResponse>(IEnumerable<TRequest> files, string UID, bool isCsv = false)
    {
        if (files == null || !files.Any())
        {
            logger.LogError(ErrorMessage.NoFileError);
            throw new Exception(ErrorMessage.NoFileError);
        }

        var tasks = new List<Task>();
        var fileToUploadToDb = new List<FileResponse>();

        foreach (var file in files)
        {
            if (file is IFormFile formFile && FileHelpers.ValidateFile(formFile, fileConfig, isCsv))
            {
                tasks.Add(formFile.ProcessFile(fileToUploadToDb, fileConfig, UID));
            }
        }
        await Task.WhenAll(tasks);

        if (typeof(TResponse) == typeof(FileResponse))
        {
            return (IEnumerable<TResponse>)(object)fileToUploadToDb;
        }

        // If you want to support mapping to another type (e.g., UploadResponse), inject IMapper and use:
        // var mapped = mapper.Map<List<TResponse>>(fileToUploadToDb);
        // return mapped;

        throw new NotSupportedException($"Unsupported TResponse type: {typeof(TResponse).Name}");
    }

    public async Task<IEnumerable<TResponse>> UploadFileWithName<TRequest, TResponse>(TRequest files, string UID, bool isCsv = false)
    {
        if (files is FileRequest fileRequest && fileRequest.FormFile != null && fileRequest.FormFile.Any())
        {
            var tasks = new List<Task>();
            var fileToUploadToDb = new List<FileResponse>();

            foreach (var file in fileRequest.FormFile)
            {
                if (FileHelpers.ValidateFile(file, fileConfig, isCsv))
                {
                    tasks.Add(file.ProcessFile(fileToUploadToDb, fileConfig, UID, fileRequest.FolderType));
                }
            }
            await Task.WhenAll(tasks);

            if (typeof(TResponse) == typeof(FileResponse))
            {
                return (IEnumerable<TResponse>)(object)fileToUploadToDb;
            }

            // If you want to support mapping to another type (e.g., UploadResponse), inject IMapper and use:
            // var mapped = mapper.Map<List<TResponse>>(fileToUploadToDb);
            // return mapped;

            throw new NotSupportedException($"Unsupported TResponse type: {typeof(TResponse).Name}");
        }
        else
        {
            logger.LogError(ErrorMessage.NoFileError);
            throw new Exception(ErrorMessage.NoFileError);
        }
    }

    public async Task<IEnumerable<TResponse>> SaveFileResponse<TResponse>(List<TResponse> fileResponse)
    {
        if (fileResponse == null || !fileResponse.Any())
            return Enumerable.Empty<TResponse>();

        if (fileResponse[0] is FileResponse fileResp)
        {
            logger.LogInfo($"Total number of files {fileResponse.Count}");
            var requestParams = new Dictionary<string, object>
            {
                { "FName", fileResp.FileName },
                { "FPath", fileResp.FilePath },
                { "FByte", fileResp.FileBytes },
                { "User_Id", fileResp.UserId }
            };

            var response = await repositoryFactory
                .ConnectDapper<FileResponse>(DbConstants.Main)
                .QueryAsync<TResponse>(FileUploadQueries.UploadFileRecords, requestParams, null);

            return response.AsEnumerable();
        }
        return Enumerable.Empty<TResponse>();
    }

    public async Task<IEnumerable<T>> GetUserId<T>(string userId, string mobileNumber, int businessId = 0)
    {
        try
        {
            var response = await repositoryFactory.ConnectDapper<string>(DbConstants.Main)
                .QueryAsync<T>(FileUploadQueries.GetUserIdWithEmailId, new Dictionary<string, object>
                {
                    { "EmailId", userId },
                    { "Mobile", mobileNumber },
                    { "BusinessId", businessId }
                }, null);
            return response.Any() ? response.AsEnumerable() : Enumerable.Empty<T>();
        }
        catch (Exception ex)
        {
            logger.LogError(ErrorMessage.GetError.FormatString(nameof(GetUserId)), ex);
            return Enumerable.Empty<T>();
        }
    }
}
