using System.Reflection;
using FluentMigrator.Runner;
using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Infrastructure.DataAccess;
using LivroDeReceitas.Infrastructure.DataAccess.Repository;
using LivroDeReceitas.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroDeReceitas.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);

            // Não precisa adicionar o contexto e nem o FluentMigrator nos testes
            if (configuration.IsUnitTestEnvironment())
                return;

            var databaseType = configuration.DatabaseType();
            if (databaseType == DatabaseType.MySql)
            {
                AddDbContextMySql(services, configuration);
                AddFluentMigratorMySql(services, configuration);
            }
            else
            {
                AddDbContextSqlSqerver(services, configuration);
                AddFluentMigratorSqlServer(services, configuration);
            }
        }


        private static void AddDbContextMySql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

            services.AddDbContext<LivroDeReceitasDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddDbContextSqlSqerver(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
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


        // Método responsável por configurar o FluentMigrator para utilizar MySQL
        private static void AddFluentMigratorMySql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            // Registra os serviços principais do FluentMigrator no container de injeção de dependência
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                    .AddMySql8()     // Define o banco de dados como MySQL 5.x
                    .WithGlobalConnectionString(connectionString)// Define a string de conexão que será usada pelas migrations

                    // Define o assembly onde estão localizadas as classes de migration
                    // Aqui ele carrega dinamicamente o assembly chamado "LivroDeReceitas.Infrastructure"
                    // e escaneia todas as classes que implementam migrations
                    .ScanIn(Assembly.Load("LivroDeReceitas.Infrastructure")).For.All();
            });
        }

        private static void AddFluentMigratorSqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("LivroDeReceitas.Infrastructure")).For.All();
            });
        }
    }
}
