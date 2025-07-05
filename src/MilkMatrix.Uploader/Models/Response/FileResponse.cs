using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Uploader.Contracts.Response;

namespace MilkMatrix.Uploader.Models.Response;
public class FileResponse : Audit, IFileResponse
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public byte[] FileBytes { get; set; }
    public string UserId { get; set; }
}

