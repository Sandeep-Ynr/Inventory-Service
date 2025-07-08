namespace MilkMatrix.Admin.Models.Admin.Requests.Page;

public class PageUpdateRequest
{
    public int? PageId { get; set; }

    public string? PageName { get; set; }

    public string? PageUrl { get; set; }

    public int? ModuleId { get; set; }

    public int? SubModuleId { get; set; }

    public int? PageOrder { get; set; }

    public string? PageIcon { get; set; }

    public bool? IsMenu { get; set; }

    public bool? Status { get; set; }
    public string? ActionDetails { get; set; }

    public int ModifyBy { get; set; }

    public bool? IsActive { get; set; }
}
