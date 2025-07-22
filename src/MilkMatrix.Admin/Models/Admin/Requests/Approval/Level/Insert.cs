namespace MilkMatrix.Admin.Models.Admin.Requests.Approval.Level;

public class Insert
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int? BusinessId { get; set; }

    public decimal? Amount { get; set; }

    public int CreatedBy { get; set; }
}
