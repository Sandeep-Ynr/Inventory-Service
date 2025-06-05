using Microsoft.Extensions.Configuration;
using MilkMatrix.DataAccess.Ado.Contracts;
using MilkMatrix.DataAccess.Ado.Implementations;
using MilkMatrix.DataAccess.Dapper.Contracts;
using MilkMatrix.DataAccess.Dapper.Implementations;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Infrastructure.Contracts.Repositories;
using MilkMatrix.Infrastructure.Models.Config;

namespace MilkMatrix.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IConfiguration configuration;

    private IConfigurationSection configurationSection;

    private string encryptKey;
    public RepositoryFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.configurationSection = configuration.GetSection(DatabaseConfig.SectionName);
        this.encryptKey = configuration.GetSection("AppConfiguration:Base64EncryptKey").Value!;
    }

    // Fix for IDE0290: Use primary constructor
    public IAdoRepository<T> Connect<T>(string connectionStringName) where T : class
    {
        var connectionString = encryptKey.DecryptString(configurationSection.GetValue<string>(connectionStringName)!);
        return new AdoRepository<T>(connectionString);
    }

    public IDapperRepository<T> ConnectDapper<T>(string connectionStringName) where T : class
    {
        var connectionString = encryptKey.DecryptString(configurationSection.GetValue<string>(connectionStringName)!);
        return new DapperRepository<T>(connectionString);
    }
}
