using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Login
{
    public class ResetPasswordModel
    {
        [Required]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        public int SecurityCode { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
