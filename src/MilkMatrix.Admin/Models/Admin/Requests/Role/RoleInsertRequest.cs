using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Role;

public class RoleInsertRequest
{
    public string RoleName { get; set; } = string.Empty;
    public int BusinessId { get; set; }
    public CrudActionType ActionType { get; set; }

    public int CreatedBy { get; set; }
}
