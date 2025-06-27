namespace MilkMatrix.Admin.Models.Login.Requests;

public class ResetPasswordRequest
{
    public string EmailId { get; set; } = string.Empty;
    public int SecurityCode { get; set; }
    public string Password { get; set; } = string.Empty;
}
