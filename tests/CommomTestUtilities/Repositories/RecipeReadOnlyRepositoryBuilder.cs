using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.Recipe;
using Moq;

namespace CommomTestUtilities.Repositories
{
    public class RecipeReadOnlyRepositoryBuilder
    {
        private readonly Mock<IRecipeReadOnlyRepository> _recipeReadOnlyRepository;

        public RecipeReadOnlyRepositoryBuilder() => _recipeReadOnlyRepository = new Mock<IRecipeReadOnlyRepository>();

        public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
        {
            _recipeReadOnlyRepository.Setup(repositoty => repositoty.Filter(user, It.IsAny<FilterRecipeDto>())).ReturnsAsync(recipes);

            return this;
        }

        public RecipeReadOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
        {
            if(recipe is not null) 
            _recipeReadOnlyRepository.Setup(repositoty => repositoty.GetById(user, recipe.Id)).ReturnsAsync(recipe);

            return this;
        }

        public IRecipeReadOnlyRepository Build() => _recipeReadOnlyRepository.Object;
    }
}
