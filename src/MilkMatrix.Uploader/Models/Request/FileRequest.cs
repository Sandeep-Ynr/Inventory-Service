using Microsoft.AspNetCore.Http;
using MilkMatrix.Uploader.Models.Enums;

namespace MilkMatrix.Uploader.Models.Request;
public class FileRequest
{
    public FileFolderType FolderType { get; set; } = FileFolderType.ProfilePath;

    public IEnumerable<IFormFile> FormFile { get; set; } = Enumerable.Empty<IFormFile>();
}
