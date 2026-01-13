using LivroDeReceitas.Domain.Repositories.Recipe;
using Moq;

namespace CommomTestUtilities.Repositories
{
    public class RecipeWriteOnlyRepositoryBuilder
    {
        public static IRecipeWriteOnlyRepository Build()
        {
            var mock = new Mock<IRecipeWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
