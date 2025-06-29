namespace MilkMatrix.Notifications.Contracts
{
    public interface IOtpService
    {
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request);
    }
}
