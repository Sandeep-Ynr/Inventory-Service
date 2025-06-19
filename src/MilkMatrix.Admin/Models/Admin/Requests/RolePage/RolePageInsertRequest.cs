using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.RolePage;

public class RolePageInsertRequest
{
    public int RoleId { get; set; }
    public int PageId { get; set; }
    public int BusinessId { get; set; }
    public string ActionDetails { get; set; } = string.Empty;

    public int CreatedBy { get; set; }
}
