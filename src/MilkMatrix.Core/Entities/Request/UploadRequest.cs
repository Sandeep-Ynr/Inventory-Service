using Microsoft.AspNetCore.Http;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Core.Entities.Request;

public class UploadRequest
{
    public FolderType FolderType { get; set; } = FolderType.ProfilePath;

    public IEnumerable<IFormFile> FormFile { get; set; } = Enumerable.Empty<IFormFile>();
}
