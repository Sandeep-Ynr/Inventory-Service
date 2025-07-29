namespace MilkMatrix.Core.Entities.Request.Approval.Details;

public class Insert
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int BusinessId { get; set; }

    public string DocNumber { get; set; }

    public string SubCode { get; set; } = string.Empty;

    public long LoginId { get; set; }
}
