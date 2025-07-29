namespace MilkMatrix.Core.Entities.Response.Approval.Details;

public class ApprovalDetails
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int BusinessId { get; set; }

    public string PageName { get; set; }
    public string BusinessName { get; set; }

    public string DocNo { get; set; }

    public string SubCode { get; set; }

    public int LoginId { get; set; }

    public DateTime ApprovalDate { get; set; }
}
