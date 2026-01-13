using LivroDeReceitas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.DataAccess
{
    // O DbContext é responsável por gerenciar a comunicação entre a aplicação e o banco de dados
    public class LivroDeReceitasDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração do DbContext.
        // Ele passa essas opções para a classe base (DbContext), que pode incluir informações como:
        // string de conexão, o tipo de banco de dados (MySQL, SQL Server, etc.), e outras configurações
        public LivroDeReceitasDbContext(DbContextOptions<LivroDeReceitasDbContext> options) : base(options)
        { }


        // DbSet que representa a tabela de usuários no banco de dados.
        // Cada DbSet corresponde a uma tabela no banco de dados e permite realizar consultas e operações nela.
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }



        // Método que permite configurar o modelo de dados, como configurações de relacionamento, chave primária, etc.
        // Isso pode incluir mapeamento de tabelas, colunas, relacionamentos e restrições de dados.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas as configurações de mapeamento de entidades para o modelo de dados, que estão no mesmo assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivroDeReceitasDbContext).Assembly);
        }
    }
}
