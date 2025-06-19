namespace MilkMatrix.Admin.Models.Admin.Responses.Page;

public class Pages
{
    public string? ActionId { get; set; }
    public int PageId { get; set; }
    public string? PageName { get; set; }
    public string? PageURL { get; set; }
    public string? PageIcon { get; set; }
    public int PageOrder { get; set; }
    public int ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? ModuleIcon { get; set; }
    public int SubModuleId { get; set; }
    public string? SubModuleName { get; set; }

    public bool IsMenu { get; set; }

    public bool IsActive { get; set; }
}
