namespace MilkMatrix.Admin.Models.Admin.Responses.SubModules;

public class SubModuleDetails
{
    public int SubModuleId { get; set; }

    public string SubModuleName { get; set; } = string.Empty;

    public int OrderNo { get; set; }

    public bool IsActive { get; set; }
}
