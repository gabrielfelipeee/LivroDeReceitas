using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Infrastructure.DataAccess;
using LivroDeReceitas.Infrastructure.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroDeReceitas.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseType = configuration.GetConnectionString("DatabaseType");

            // Convertendo uma string para um valor do enum DatabaseType
            var databaseTypeEnum = (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);

            if (databaseTypeEnum == DatabaseType.MySql)
            {
                AddDbContextMySql(services, configuration);
            }
            else
            {
                AddDbContextSqlSqerver(services, configuration);
            }

            AddRepositories(services);
        }


        private static void AddDbContextMySql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionMySQLServer");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

            services.AddDbContext<LivroDeReceitasDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddDbContextSqlSqerver(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionSQLServer");
            services.AddDbContext<LivroDeReceitasDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        }
    }
}
