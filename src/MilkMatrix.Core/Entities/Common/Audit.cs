namespace MilkMatrix.Core.Entities.Common;
public class Audit
{
    public bool? IsActive { get; set; }
    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifyOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifyBy { get; set; }
}
