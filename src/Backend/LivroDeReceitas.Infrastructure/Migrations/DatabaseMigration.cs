using Dapper;
using FluentMigrator.Runner;
using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace LivroDeReceitas.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static void Migrate(DatabaseType databaseType, IServiceProvider serviceProvider, string connectionString)
        {
            if (databaseType == DatabaseType.MySql)
                EnsureDatabaseCreatedMySql(connectionString);
            else
                EnsureDatabaseCreatedSqlServer(connectionString);

            MigrationDatabase(serviceProvider);
        }

        // Método para garantir que o banco de dados seja criado no MySQL, caso não exista.
        private static void EnsureDatabaseCreatedMySql(string connectionString)
        {
            // Cria um objeto que interpreta a connection string.
            // facilita o acesso a partes da string, como o nome do banco (Database), usuário, servidor etc.
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

            // Obtém o nome do banco de dados a partir da string de conexão.
            var databaseName = connectionStringBuilder.Database;

            // Removendo o nome do db para não dá erro caso ele não exista
            connectionStringBuilder.Remove("Database");

            // Abre uma nova conexão com o banco de dados MySQL usando a string de conexão fornecida.
            // O using var garante que a conexão será fechada automaticamente ao final do bloco
            using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

            // Cria um objeto de parâmetros para passar valores com segurança em uma query (usando o Dapper).
            var parameters = new DynamicParameters();

            // Adiciona o nome do banco ao objeto de parâmetros
            parameters.Add("dbName", databaseName);

            // Executa uma consulta SQL para verificar se o banco de dados já existe, consultando o esquema do banco de dados.
            var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbName", parameters);
            if (records.Any().IsFalse())
                dbConnection.Execute($"CREATE DATABASE {databaseName}");  // Cria o banco de dados com o nome especificado na string de conexão.
        }


        private static void EnsureDatabaseCreatedSqlServer(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.Remove("Initial Catalog");

            using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("dbName", databaseName);

            var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @dbName", parameters);
            if (records.Any().IsFalse())
                dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }


        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            // Esse é o objeto que sabe como executar as migrations
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Lista todas as migrations encontradas
            runner.ListMigrations();

            // Executa as migrations pendentes (que ainda não foram aplicadas no banco)
            runner.MigrateUp();
        }
    }
}
