using MilkMatrix.Core.Abstractions.Notification;
using MilkMatrix.Notifications.Contracts;

namespace MilkMatrix.Infrastructure.Common.Notifications;

public class NotificationAdapter(IOtpService notification) : INotificationService
{
    public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request) => notification.SendAsync<TRequest, TResponse>(request);
}
