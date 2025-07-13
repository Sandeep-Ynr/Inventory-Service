using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Login
{
    public class ChangePasswordModel
    {
        public string? EmailId { get; set; }

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        public string OldPassword { get; set; } = string.Empty;
    }
}
