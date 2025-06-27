namespace MilkMatrix.Core.Abstractions.Notification;

public interface INotificationService
{
    Task<TResponse> SendOtpAsync<TRequest, TResponse>(TRequest request);
}
