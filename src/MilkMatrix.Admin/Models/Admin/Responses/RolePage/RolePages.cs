using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.RolePage;

public class RolePages
{
    public int PageId { get; set; }
    
    [GlobalSearch]
    public string PageName { get; set; } = string.Empty;
    public string PageActionId { get; set; } = string.Empty;

    public string RoleActionId { get; set; } = string.Empty;
}
