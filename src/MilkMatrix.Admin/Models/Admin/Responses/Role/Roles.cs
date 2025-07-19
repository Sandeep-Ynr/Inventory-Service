using MilkMatrix.Core.Attributes;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Admin.Responses.Role;
public class Roles : Audit
{
    public int Id { get; set; }
    
    [GlobalSearch]
    public string? Name { get; set; }
}
