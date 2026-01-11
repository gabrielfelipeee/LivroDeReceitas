using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.Recipe;

namespace LivroDeReceitas.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeWriteOnlyRepository
    {
        private readonly LivroDeReceitasDbContext _dbContext;

        public RecipeRepository(LivroDeReceitasDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
    }
}
