namespace MilkMatrix.Api.Models.Request.Login
{
    public class LoginModel
    {
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public int? BusinessId { get; set; }
        public int? Otp { get; set; }
        public MobileAppFields? MobileAppFields { get; set; }
    }
    
}
