using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Services.LoggedUser;
using Moq;

namespace CommomTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User userEntity)
        {
            // Cria um mock da interface ILoggedUser
            var mock = new Mock<ILoggedUser>();

            // Configura o mock:
            // Sempre que alguém chamar o método User() dessa interface,
            // ele vai retornar, de forma assíncrona, o "userEntity" passado como parâmetro
            mock.Setup(x => x.User()).ReturnsAsync(userEntity);

            // Retorna o objeto mockado
            return mock.Object;
        }
    }
}
