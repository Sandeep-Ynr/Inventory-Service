using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Login;

public class RefreshTokenModel
{
    [Required(ErrorMessage = "Refresh token is required!!")]
    public string RefreshToken { get; set; }
}
