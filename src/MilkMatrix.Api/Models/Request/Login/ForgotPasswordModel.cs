using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Login
{
    public class ForgotPasswordModel
    {
        [Required]
        public string EmailId { get; set; }
    }
}
