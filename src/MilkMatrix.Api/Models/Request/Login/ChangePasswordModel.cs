namespace MilkMatrix.Api.Models.Request.Login
{
    public class ChangePasswordModel
    {
        public string? UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
