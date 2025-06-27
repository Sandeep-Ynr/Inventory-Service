namespace MilkMatrix.Core.Entities.Response;

public class TokenStatusResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }

    public string? UserId { get; set; }

    public string? Mobile { get; set; }

    public int? BusinessId { get; set; }
}
