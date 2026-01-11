using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.User;
using Moq;

namespace CommomTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _repository;

        public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

        public UserUpdateOnlyRepositoryBuilder GetById(User userEntity)
        {
            _repository.Setup(x=> x.GetById(userEntity.Id)).ReturnsAsync(userEntity);

            // Retorna a própria instância do builder para permitir o encadeamento de chamadas (Fluent Interface).
            // Exemplo disso: new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            // Sem o "return this", seria necessário quebrar em múltiplas linhas e perder fluidez.
            return this;
        }

        public IUserUpdateOnlyRepository Build() => _repository.Object;
    }
}
