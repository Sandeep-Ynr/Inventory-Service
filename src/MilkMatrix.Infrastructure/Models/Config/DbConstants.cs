using Microsoft.Extensions.Configuration;

namespace MilkMatrix.Infrastructure.Models.Config
{
    public static class DbConstants
    {
        public const string Main = "MainConnectionString";
        public const string Report = "RptConnectionString";
        public const string Default = "DefaultConnectionString";
    }
}
