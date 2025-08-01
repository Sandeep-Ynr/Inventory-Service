namespace MilkMatrix.Api.Models.Request.Admin.Page;

public class PageUpsertModel
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public int? Module { get; set; }

    public int? SubModule { get; set; }

    public int? Order { get; set; }  

    public string? Icon { get; set; }

    public bool? IsMenu { get; set; }

    public bool? Status { get; set; }

    public int? ParentId { get; set; }
    public string? ActionDetails { get; set; }
}
