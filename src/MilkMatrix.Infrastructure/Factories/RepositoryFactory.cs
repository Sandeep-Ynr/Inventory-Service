using Microsoft.Extensions.Configuration;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.DataAccess.Ado.Implementations;
using MilkMatrix.DataAccess.Dapper.Implementations;
using MilkMatrix.Infrastructure.Common.Utils;

namespace MilkMatrix.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly ILogging logger;
    private readonly IConfiguration configuration;
    private IConfigurationSection configurationSection;
    private string encryptKey;
    public RepositoryFactory(IConfiguration configuration, ILogging logger)
    {
        this.configuration = configuration;
        this.configurationSection = configuration.GetSection(DatabaseConfig.SectionName);
        this.encryptKey = configuration.GetSection("AppConfiguration:Base64EncryptKey").Value!;
        this.logger = logger.ForContext("ServiceName", nameof(RepositoryFactory));
    }

    public IBaseRepository<T> Connect<T>(string connectionStringName) where T : class
    {
        var connectionString = encryptKey.DecryptString(configurationSection.GetValue<string>(connectionStringName)!);
        return new AdoRepository<T>(connectionString, logger);
    }

    public IBaseRepository<T> ConnectDapper<T>(string connectionStringName) where T : class
    {
        //var econnectionString = encryptKey.EncryptString("server=DESKTOP-KB3J5PN\\SQLEXPRESS;database=MMBeta;user id=prince;password=jks1988@1122;Min Pool Size=10;Max Pool Size=200;Connect Timeout=1000; trusted_connection=true;Encrypt=false;");
        var connectionString = encryptKey.DecryptString(configurationSection.GetValue<string>(connectionStringName)!);
        return new DapperRepository<T>(connectionString, logger);
    }
}
