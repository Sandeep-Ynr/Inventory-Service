namespace MilkMatrix.Api.Models.Request.Admin.Module;

public class ModuleUpsertModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public int? Order { get; set; }

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }
}
