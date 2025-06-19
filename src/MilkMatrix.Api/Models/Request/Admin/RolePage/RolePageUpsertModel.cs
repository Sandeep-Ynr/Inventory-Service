using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.RolePage
{
    public class RolePageUpsertModel
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int PageId { get; set; }
        [Required]
        public int BusinessId { get; set; }
        public string ActionDetails { get; set; } = string.Empty;
    }
}
