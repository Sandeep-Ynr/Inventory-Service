namespace MilkMatrix.Notifications.Contracts
{
    public interface IOtpService
    {
        Task<TResponse> SendOtpAsync<TRequest, TResponse>(TRequest request);
    }
}
