using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.SubModule;

public class SubModuleInsertRequest
{
    public string Name { get; set; } = string.Empty;

    public int Order { get; set; } = 1;

    public CrudActionType ActionType { get; set; } = CrudActionType.Create;

    public int CreatedBy { get; set; }
}
