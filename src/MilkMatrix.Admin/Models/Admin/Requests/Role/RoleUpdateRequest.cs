using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Role;

public class RoleUpdateRequest
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public int? BusinessId { get; set; }
    public CrudActionType ActionType { get; set; }

    public int ModifyBy { get; set; }

    public bool? IsActive { get; set; } = true;
}
