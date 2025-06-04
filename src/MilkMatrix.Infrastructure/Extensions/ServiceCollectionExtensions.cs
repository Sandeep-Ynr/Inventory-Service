using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MilkMatrix.DataAccess.Ado.Contracts;
using MilkMatrix.DataAccess.Ado.Implementations;
using MilkMatrix.Infrastructure.Common.Logger.Implementation;
using MilkMatrix.Infrastructure.Common.Logger.Interface;
using MilkMatrix.Infrastructure.Contracts.Repositories;
using MilkMatrix.Infrastructure.Factories;
using MilkMatrix.Infrastructure.Models.Config;
using MilkMatrix.Logging.Config;
using MilkMatrix.Logging.Extensions;

namespace MilkMatrix.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IHostBuilder AddInfra(this IHostBuilder hostBuilder, string? logFilePath = null)
        {
            // Register any infrastructure services here  
            // For example, logging, caching, etc.  
            hostBuilder.AddLogging(logFilePath);
            return hostBuilder;
        }

        public static IServiceCollection ConfigureInfraservices(this IServiceCollection services, IConfiguration configuration) =>
            services
              .AddSingleton<IRepositoryFactory, RepositoryFactory>()
              .RegisterLoggingDependencies()
              .AddSingleton<ILogging, LoggingAdapter>()
              .AddDataAccess(configuration);

        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration) =>
            // This method can be used to register configuration settings if needed
            // For example, you can register configuration sections or bind them to classes
            services
            .Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.SectionName))
            .Configure<LoggerConfig>(configuration.GetSection(LoggerConfig.SectionName))
            .Configure<AppConfig>(configuration.GetSection(AppConfig.SectionName));

        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration) =>
            services.AddScoped(typeof(IAdoRepository<>), provider =>
            {
                var connectionString = configuration.GetConnectionString("MainConnectionString");
                return (Type repoType) =>
                {
                    var entityType = repoType.GenericTypeArguments[0];
                    var adoRepoType = typeof(AdoRepository<>).MakeGenericType(entityType);
                    return Activator.CreateInstance(adoRepoType, connectionString);
                };
            });
    }
}
