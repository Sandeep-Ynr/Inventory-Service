using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Uploader.Contracts.Services;
using MilkMatrix.Uploader.Implementation.Services;
using MilkMatrix.Uploader.Models.Config;

namespace MilkMatrix.Uploader.Common.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUploaderServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
         .Configure<UploadConfig>(configuration.GetSection(UploadConfig.SectionName))
         .Configure<FileConfig>(configuration.GetSection(FileConfig.SectionName))
                .AddTransient<IUploadService, UploadService>();
    }
}
