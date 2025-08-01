using MilkMatrix.Core.Attributes;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Admin.Responses.SubModules;

public class SubModuleDetails : Audit
{
    public int SubModuleId { get; set; }

    [GlobalSearch]
    public string SubModuleName { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    public int OrderNo { get; set; }

    public bool IsActive { get; set; }
}
