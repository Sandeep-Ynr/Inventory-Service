namespace MilkMatrix.Admin.Models.Admin.Responses.Page;

public class PageList
{
    public string? ActionId { get; set; }
    public int RoleId { get; set; }
    public int PageId { get; set; }
    public string? PageName { get; set; }
    public string? PageURL { get; set; }
    public string? PageIcon { get; set; }
    public int PageOrder { get; set; }
    public int ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? ModuleIcon { get; set; }
    public int ModuleOrderNumber { get; set; }
    public int SubModuleId { get; set; }
    public string? SubModuleName { get; set; }
    public int SubModuleOrderNumber { get; set; }
    public IEnumerable<Actions>? ActionList { get; set; }

    public int? ParentId { get; set; }

    public int? SubModuleParentId { get; set; }

    public List<PageList>? Children { get; set; } // Add this for tree building 
}
