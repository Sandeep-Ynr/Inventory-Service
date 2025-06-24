using MilkMatrix.Admin.Models.Admin.Responses.Page;

namespace MilkMatrix.Admin.Models.Admin.Responses.SubModules;

public class SubModule
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int OrderNumber { get; set; }
    public IEnumerable<PageList>? PageList { get; set; }
}
