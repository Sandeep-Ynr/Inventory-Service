namespace MilkMatrix.Admin.Models.Login.Response;

public class LoginMasterResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public LoginResponse? Data { get; set; }
}
