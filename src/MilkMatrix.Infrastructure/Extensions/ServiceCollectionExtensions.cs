using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MilkMatrix.Core.Abstractions.Csv;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.HostedServices;
using MilkMatrix.Core.Abstractions.HttpClient;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Notification;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Abstractions.Uploader;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Infrastructure.Common.Csv;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Infrastructure.Common.HostedServices;
using MilkMatrix.Infrastructure.Common.Logger.Implementation;
using MilkMatrix.Infrastructure.Common.Notifications;
using MilkMatrix.Infrastructure.Common.Uploader;
using MilkMatrix.Infrastructure.Factories;
using MilkMatrix.Logging.Config;
using MilkMatrix.Logging.Extensions;
using MilkMatrix.Notifications.Common.Extensions;
using MilkMatrix.Notifications.Models.Config;
using MilkMatrix.Uploader.Common.Extensions;

namespace MilkMatrix.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IHostBuilder AddInfraLogging(this IHostBuilder hostBuilder, string? logFilePath = null) => hostBuilder.AddLogging(logFilePath);

        public static IServiceCollection AddLoggingServices(this IServiceCollection services) => services.RegisterLoggingDependencies();

        public static IHostBuilder AddInfra(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services
                    .ConfigureInfraservices(context.Configuration)
                    .AddUploaderServices(context.Configuration);
            });
            return hostBuilder;
        }

        public static IServiceCollection ConfigureInfraservices(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSingleton<ILogging, LoggingAdapter>()
                .AddHttpClient()
                .AddScoped<IClientFactory, ClientFactory>()
                .AddScoped<ICsvReader, CsvFileReader>()
                .AddScoped<INotificationService, NotificationAdapter>()
                .AddScoped<IFileUploader, UploadProvider>()
                .AddNotificationServices(configuration)
                .AddDataAccess()
                .AddScoped<IQueryMultipleData, QueryMultipleData>()
                .AddBackgroundProcessing();

        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.SectionName))
                .Configure<LoggerConfig>(configuration.GetSection(LoggerConfig.SectionName))
                .Configure<AppConfig>(configuration.GetSection(AppConfig.SectionName))
                .Configure<SMSConfig>(configuration.GetSection(SMSConfig.SectionName))
                .Configure<EmailConfig>(configuration.GetSection(EmailConfig.SectionName));

        public static IServiceCollection AddDataAccess(this IServiceCollection services) =>
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

        public static IServiceCollection AddBackgroundProcessing(this IServiceCollection services)
        {
            services.AddSingleton<IBulkProcessingTasks, BulkProcessingTasks>();
            services.AddSingleton<IBulkHostedService, BulkHostedService>();
            services.AddHostedService(provider => (BulkHostedService)provider.GetRequiredService<IBulkHostedService>());
            return services;
        }
    }
}
