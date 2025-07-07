using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Core.Entities.Response;

public class UploadResponse :Audit, IUploadResponse
{
    public int FileId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public byte[] FileBytes { get; set; }
    public int UserId { get; set; }
}
