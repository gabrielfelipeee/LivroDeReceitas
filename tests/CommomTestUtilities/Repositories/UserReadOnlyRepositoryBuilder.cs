using LivroDeReceitas.Domain.Repositories.User;
using Moq;

namespace CommomTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();


        // Retorna false, pois é o valor padrão de Bool
        public IUserReadOnlyRepository Build() => _repository.Object; 


        // Configurado para retornar True
        public void ExistActiveUserWithEmail(string email)
        {
            var emailExist = _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }
    }
}
