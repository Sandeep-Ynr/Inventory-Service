using MilkMatrix.Admin.Models.Admin.Responses.SubModules;

namespace MilkMatrix.Admin.Models.Admin.Responses.Modules;

public class Module
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public int OrderNumber { get; set; }
    public IEnumerable<SubModule>? SubModuleList { get; set; }
}
