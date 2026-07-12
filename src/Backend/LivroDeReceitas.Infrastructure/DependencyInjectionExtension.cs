using FluentMigrator.Runner;
using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Security.Cryptography;
using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.OpenAI;
using LivroDeReceitas.Infrastructure.DataAccess;
using LivroDeReceitas.Infrastructure.DataAccess.Repositories;
using LivroDeReceitas.Infrastructure.Extensions;
using LivroDeReceitas.Infrastructure.Security.Cryptography;
using LivroDeReceitas.Infrastructure.Security.Tokens.Access.Generator;
using LivroDeReceitas.Infrastructure.Security.Tokens.Access.Validator;
using LivroDeReceitas.Infrastructure.Services.LoggedUser;
using LivroDeReceitas.Infrastructure.Services.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using OpenAI.Chat;
using LivroDeReceitas.Domain.ValueObjects;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Infrastructure.Services.Storage;
using Azure.Storage.Blobs;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Infrastructure.Services.ServiceBus;
using Azure.Messaging.ServiceBus;
using LivroDeReceitas.Domain.Services.ServiceBus;

namespace LivroDeReceitas.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddTokens(services, configuration);
            AddLoggedUser(services);
            AddPasswordEncripter(services);
            AddOpenAI(services, configuration);
            AddAzureStorage(services, configuration);
            AddQueue(services, configuration);

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
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();

            services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
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

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signinKey!));
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

        private static void AddPasswordEncripter(IServiceCollection services)
        {
            services.AddScoped<IPasswordEncripter, BCryptNet>();
        }

        private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

            var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

            services.AddScoped(c => new ChatClient(LivroDeReceitasRuleConstants.CHAT_MODEL, apiKey));
        }

        private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

            if (connectionString.NotEmpty())
                services.AddScoped<IBlobStorageService>(service => new AzureStorageService(new BlobServiceClient(connectionString)));
        }

        private static void AddQueue(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });


            var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            }));
            services.AddSingleton(deleteUserProcessor);


            var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));
            services.AddScoped<IDeleteUserQueue>(service => deleteQueue);
        }
    }
}
