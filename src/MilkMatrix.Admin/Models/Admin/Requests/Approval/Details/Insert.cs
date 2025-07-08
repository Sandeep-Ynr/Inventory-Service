namespace MilkMatrix.Admin.Models.Admin.Requests.Approval.Details;

public class Insert
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int BusinessId { get; set; }

    public string DocNumber { get; set; }

    public string SubCode { get; set; }

    public long LoginId { get; set; }
}
