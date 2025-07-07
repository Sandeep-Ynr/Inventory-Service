using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Uploader.Models.Response;

public class FileDtoResponse : Audit
{
    public int FileId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
}
