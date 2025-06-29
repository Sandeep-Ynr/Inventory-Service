namespace MilkMatrix.Core.Abstractions.Notification;

public interface INotificationService
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request);
}
