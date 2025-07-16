using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Approval.Level;

public class ApprovalResponse
{
    public int UserId { get; set; }

    public int PageId { get; set; }

    public int Level { get; set; }

    public int BusinessId { get; set; }

    [GlobalSearch]
    public string PageName { get; set; }

    [GlobalSearch]
    public string BusinessName { get; set; }

    public string UserName { get; set; }

    public string HrmsCode { get; set; }

    public bool IsActive { get; set; }
}
