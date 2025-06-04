namespace MilkMatrix.Domain.Entities.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}
