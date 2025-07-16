using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Role;
public class Roles
{
    public int Id { get; set; }
    
    [GlobalSearch]
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}
