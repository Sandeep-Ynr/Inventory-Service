using AutoMapper;
using MilkMatrix.Core.Abstractions.Notification;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Notifications.Contracts;
using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;
using MilkMatrix.Uploader.Implementation.Services;
using MilkMatrix.Uploader.Models.Request;
using MilkMatrix.Uploader.Models.Response;

namespace MilkMatrix.Infrastructure.Common.Notifications;

public class NotificationAdapter(IOtpService notification, IMapper mapper) : INotificationService
{
    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
    {
        // Only support UploadRequest -> UploadResponse or FileRequest -> FileResponse
        if (typeof(TRequest) == typeof(OTPRequest) && typeof(TResponse) == typeof(OTPResponse))
        {
            return await notification.SendAsync<TRequest, TResponse>(request);
        }
        if (typeof(TRequest) == typeof(NotificationRequest) && typeof(TResponse) == typeof(NotificationResponse))
        {
            var requests = mapper.Map<OTPRequest>(request);
            var responses = await notification.SendAsync<OTPRequest, OTPResponse>(requests);
            var mapped = mapper.Map<NotificationResponse>(responses);
            return (TResponse)(object)mapped;
        }
        throw new NotSupportedException("Unsupported type combination for UploadFile.");
    }
}
