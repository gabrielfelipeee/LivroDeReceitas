using Moq;
using LivroDeReceitas.Domain.Repositories;


namespace CommomTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            var mock = new Mock<IUnitOfWork>(); // Cria uma imlementação fake
            return mock.Object;
        }
    }
}
