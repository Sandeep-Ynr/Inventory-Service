using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.Role
{
    public class RoleDetails : Roles
    {
        public int BusinessId { get; set; }
        
        [GlobalSearch]
        public string? BusinessName { get; set; }
    }
}
