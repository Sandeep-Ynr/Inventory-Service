namespace MilkMatrix.Admin.Models.Admin.Responses.Modules;

public class ModuleDetails
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = string.Empty;

    public string? ModuleIcon { get; set; }

    public int OrderNo { get; set; }

    public bool IsActive { get; set; }
}
