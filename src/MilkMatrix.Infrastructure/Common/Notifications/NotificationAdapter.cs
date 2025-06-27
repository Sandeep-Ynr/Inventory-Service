using MilkMatrix.Core.Abstractions.Notification;
using MilkMatrix.Notifications.Contracts;

namespace MilkMatrix.Infrastructure.Common.Notifications;

public class NotificationAdapter(IOtpService notification) : INotificationService
{
    public Task<TResponse> SendOtpAsync<TRequest, TResponse>(TRequest request) => notification.SendOtpAsync<TRequest, TResponse>(request);
}
