using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.Module;

public class ModuleInsertRequest
{
    public string Name { get; set; } = string.Empty;

    public int Order { get; set; } = 1;

    public string? Icon { get; set; }

    public CrudActionType ActionType { get; set; } = CrudActionType.Create;


    public int CreatedBy { get; set; }
}
