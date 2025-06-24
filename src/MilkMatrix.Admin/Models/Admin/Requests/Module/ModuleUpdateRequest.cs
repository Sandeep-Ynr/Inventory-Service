using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Module;

public class ModuleUpdateRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int? Order { get; set; }

    public string? Icon { get; set; }

    public CrudActionType ActionType { get; set; } = CrudActionType.Update;


    public int ModifyBy { get; set; }

    public bool IsAcive { get; set; }
}
