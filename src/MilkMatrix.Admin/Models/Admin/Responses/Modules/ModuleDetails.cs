using MilkMatrix.Core.Attributes;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Admin.Responses.Modules;

public class ModuleDetails : Audit
{
    public int ModuleId { get; set; }
    [GlobalSearch]
    public string ModuleName { get; set; } = string.Empty;

    public string? ModuleIcon { get; set; }

    public int OrderNo { get; set; }

    public bool IsActive { get; set; }
}
