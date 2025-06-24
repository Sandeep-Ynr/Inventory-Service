using MilkMatrix.Admin.Models.Admin.Responses.Page;

namespace MilkMatrix.Admin.Models.Admin.Responses.Modules;

public class ModuleResponse
{
    public IEnumerable<Module>? ModuleList { get; set; }
    public IEnumerable<PageList>? PageList { get; set; }
}
