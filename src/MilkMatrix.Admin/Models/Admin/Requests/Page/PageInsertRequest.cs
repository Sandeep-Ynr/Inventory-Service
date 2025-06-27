namespace MilkMatrix.Admin.Models.Admin.Requests.Page;

public class PageInsertRequest
{
    public string PageName { get; set; } = string.Empty;

    public string PageUrl { get; set; } = string.Empty;

    public int ModuleId { get; set; }

    public int SubModuleId { get; set; }

    public int PageOrder { get; set; }

    public string PageIcon { get; set; } = string.Empty;

    public bool IsMenu { get; set; }

    public string ActionDetails { get; set; } = string.Empty;

    public int CreatedBy { get; set; }
}
