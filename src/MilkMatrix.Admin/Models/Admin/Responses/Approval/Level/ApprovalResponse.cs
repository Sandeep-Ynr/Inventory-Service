namespace MilkMatrix.Admin.Models.Admin.Responses.Approval.Level;

public class ApprovalResponse
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int BusinessId { get; set; }

    public int Amount { get; set; }

    public int DepartmentId { get; set; }

    public bool IsActive { get; set; }
}
