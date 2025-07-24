using CommomTestUtilities.Entities;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private UserEntity _userEntity = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test") // Define o ambiente como "Test", isolando a configuração dos testes
                .ConfigureServices(services =>
                {
                    // Vai até os serviços de DI e verifica se existe o DbContextOptions<LivroDeReceitasDbContext>
                    var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<LivroDeReceitasDbContext>));

                    // Se encontrar, remove a configuração original (ligada ao banco real)
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    // Cria um provedor de serviços para o banco de dados em memória
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    // Registra o DbContext usando um banco de dados em memória
                    services.AddDbContext<LivroDeReceitasDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting"); // Nome do banco em memória
                        options.UseInternalServiceProvider(provider); // Usa o provedor criado acima
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<LivroDeReceitasDbContext>();

                    dbContext.Database.EnsureDeleted(); // Garante que o db inicie vazio

                    StartDatabase(dbContext);
                });
        }

        public string GetEmail() => _userEntity.Email;
        public string GetName() => _userEntity.Name;
        public string GetPassword() => _password;

        private void StartDatabase(LivroDeReceitasDbContext dbContext)
        {
            (_userEntity, _password) = UserEntityBuilder.Build();

            dbContext.Users.Add(_userEntity); // Adiciona um usuário ao banco

            dbContext.SaveChanges();
        }
    }
}






