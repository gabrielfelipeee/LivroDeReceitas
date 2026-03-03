using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.Recipe;
using Moq;

namespace CommomTestUtilities.Repositories
{
    public class RecipeUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IRecipeUpdateOnlyRepository> _recipeUpdateOnlyRepository;
        public RecipeUpdateOnlyRepositoryBuilder() => _recipeUpdateOnlyRepository = new Mock<IRecipeUpdateOnlyRepository>();

        public RecipeUpdateOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
        {
            if (recipe is not null)
                _recipeUpdateOnlyRepository.Setup(repositoty => repositoty.GetById(user, recipe.Id)).ReturnsAsync(recipe);

            return this;
        }

        public IRecipeUpdateOnlyRepository Build() => _recipeUpdateOnlyRepository.Object;
    }
}
