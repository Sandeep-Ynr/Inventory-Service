using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Notifications.Contracts;
using MilkMatrix.Notifications.Implementation;

namespace MilkMatrix.Notifications.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddScoped<IOtpService, OtpService>()
        .AddScoped<IEmailService, EmailService>()
         .AddScoped<ISMSService, SMSService>();
}
