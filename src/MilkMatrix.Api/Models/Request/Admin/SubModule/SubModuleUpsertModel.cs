namespace MilkMatrix.Api.Models.Request.Admin.SubModule;

public class SubModuleUpsertModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public int? Order { get; set; } = 1;

    public bool? IsAcive { get; set; } = true;
}
