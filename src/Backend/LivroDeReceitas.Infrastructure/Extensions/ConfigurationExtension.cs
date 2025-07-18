using LivroDeReceitas.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace LivroDeReceitas.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static DatabaseType DatabaseType(this IConfiguration configuration)
        {
            var databaseType = configuration.GetConnectionString("DatabaseType");

            // Convertendo uma string para um valor do enum DatabaseType
            return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
        }

        public static string ConnectionString(this IConfiguration configuration)
        {
            var databaseType = configuration.DatabaseType(); // O método de extensão acima

            if (databaseType == Domain.Enums.DatabaseType.MySql)
                return configuration.GetConnectionString("ConnectionMySQLServer")!;
            else
                return configuration.GetConnectionString("ConnectionSQLServer")!;
        }
    }
}
