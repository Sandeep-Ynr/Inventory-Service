using Microsoft.AspNetCore.Http;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Helpers;
using MilkMatrix.Uploader.Models.Enums;
using MilkMatrix.Uploader.Models.Response;
using static MilkMatrix.Uploader.Common.Constants;

namespace MilkMatrix.Uploader.Helpers
{
  public static class FileHelpers
  {
    // add a check to validate whether file is actually pdf/jpg/png or not supported crc
    // folders structure to be as same as detaservices
    public static bool ValidateFile(IFormFile file, FileConfig config, bool isCsv = false)
    {
      // Maximum file size needs to be moved to config
      if (file.Length > config.MaximumFileLength) // 5 MB
        throw new Exception(string.Format(ErrorMessage.FileLengthError, file.FileName));

      // file extensions check
      // var allowedExtensions = config.AllowedExtensions;
      // new[] { ".pdf", ".jpg", ".jpeg", ".png" };
      // var fileExtension = Path.GetExtension(file.FileName).ToLower();
      if (!isCsv && !config.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        throw new Exception(string.Format(ErrorMessage.UnsupportedFileExtension, file.FileName));
      return true;
    }

    private static async Task<byte[]> GetBytesFromFileAsync(this IFormFile file)
    {
      byte[]? fileBytes = null;

      await using (var ms = new MemoryStream())
      {
        await file.CopyToAsync(ms);
        fileBytes = ms.ToArray();
      }
      return fileBytes;
    }

    public static async Task ProcessFile(this IFormFile file,
        List<FileResponse> fileToUploadToDb,
        FileConfig fileConfig,
        string UID,
        FileFolderType fileFolderType = FileFolderType.ProfilePath)
    {
      string fileName, filePath;
      GetFileNameAndFilePath(file, fileConfig, out fileName, out filePath, fileFolderType);

      fileToUploadToDb.Add(new FileResponse() { FileName = fileName, FileBytes = await file.GetBytesFromFileAsync(), FilePath = filePath, UserId = UID });
      var destinationPath = Path.Combine(fileConfig.UploadFileDirectoryPath, filePath);
      destinationPath = Path.Combine(Directory.GetCurrentDirectory(), destinationPath);
      using var memoryStream = new MemoryStream();
      await file.CopyToAsync(memoryStream);

      // Reset the position to the beginning of the MemoryStream
      memoryStream.Seek(0, SeekOrigin.Begin);

      // If you want to save the bytes to a file
      using var fileStream = new FileStream(destinationPath, FileMode.OpenOrCreate);
      await memoryStream.CopyToAsync(fileStream);
    }
    private static void GetFileNameAndFilePath(IFormFile file,
        FileConfig fileConfig,
        out string fileName,
        out string filePath,
        FileFolderType fileFolderType = FileFolderType.ProfilePath
        )
    {
      var extension = Path.GetExtension(file.FileName);
      // Save the file to a directory or database path to directory to be moved to config
      fileName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + Guid.NewGuid() + extension;
      var actionType = extension.SetActionType();
      filePath = fileConfig.GetUploadFilePath(actionType, fileFolderType);
      filePath = Path.Combine(filePath, fileName);
    }

    private static FilePathActionType SetActionType(this string extension) => extension switch
    {
      ".csv" => FilePathActionType.Csv,
      ".pdf" => FilePathActionType.Pdf,
      ".png" or ".jpg" or ".jpeg" => FilePathActionType.Image,
      _ => FilePathActionType.FileMedia,
    };
    private static string GetUploadFilePath(this FileConfig fileConfig,
        FilePathActionType ActionType,
        FileFolderType fileFolderType)
    {
      var filePath = string.Empty;
      var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileConfig.UploadFileDirectoryPath);
      filePath = GetFolderHierarchy(fileConfig, fileFolderType, ref filePath);
      switch (ActionType)
      {
        case FilePathActionType.Image:
          filePath += fileConfig.ImageFilePath;
          return filePath.CheckGenerateSubFolderHierarchy(destinationPath);
        case FilePathActionType.FileMedia:
          filePath += fileConfig.FileMediaPath;
          return filePath.CheckGenerateSubFolderHierarchy(destinationPath);
        case FilePathActionType.Csv:
          filePath += fileConfig.CsvFilePath;
          return filePath.CheckGenerateSubFolderHierarchy(destinationPath);
        case FilePathActionType.Pdf:
          filePath += fileConfig.PdfFilePath;
          return filePath.CheckGenerateSubFolderHierarchy(destinationPath);
        default:
          filePath += fileConfig.DocsFilePath;
          return filePath.CheckGenerateSubFolderHierarchy(destinationPath);
      }
    }

    private static string GetFolderHierarchy(FileConfig fileConfig, FileFolderType fileFolderType, ref string filePath)
    {
      switch (fileFolderType)
      {
        case FileFolderType.ProfilePath:
          filePath = fileConfig.ProfilePath;
          return filePath;
        case FileFolderType.InventoryPath:
          filePath = fileConfig.InventoryPath;
          return filePath;
        case FileFolderType.VisitorPath:
          filePath = fileConfig.VisitorPath;
          return filePath;
        case FileFolderType.FileMediaPath:
          filePath = fileConfig.FileMediaPath;
          return filePath;
        case FileFolderType.MobileAppPath:
          filePath = fileConfig.MobileAppPath;
          return filePath;
        case FileFolderType.PayRollPath:
          filePath = fileConfig.PayRollPath;
          return filePath;
        case FileFolderType.AccountsPath:
          filePath = fileConfig.AccountsPath;
          return filePath;
        case FileFolderType.PurchasePath:
          filePath = fileConfig.PurchasePath;
          return filePath;
        case FileFolderType.SalesPath:
          filePath = fileConfig.SalesPath;
          return filePath;
        case FileFolderType.DownloadPdfPath:
          filePath = fileConfig.DownloadPDFPath;
          return filePath;
        default:
          filePath = fileConfig.ProfilePath;
          return filePath;
      }
    }
  }
}
