namespace MilkMatrix.Core.Entities.Common;
public class Audit
{
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}
