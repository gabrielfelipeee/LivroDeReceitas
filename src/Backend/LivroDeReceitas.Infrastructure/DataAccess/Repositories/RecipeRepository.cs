using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeUpdateOnlyRepository, IRecipeReadOnlyRepository
    {
        private readonly LivroDeReceitasDbContext _dbContext;

        public RecipeRepository(LivroDeReceitasDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
        public async Task Delete(long recipeId)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            _dbContext.Recipes.Remove(recipe!);
        }



        public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);
        async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
            => await GetFullRecipeById(user, recipeId).FirstOrDefaultAsync();



        public async Task<IList<Recipe>> Filter(User user, FilterRecipeDto filters)
        {

            IQueryable<Recipe> query = _dbContext.Recipes
                .AsNoTracking()
                .Include(recipe => recipe.Ingredients)
                .Where(recipe => recipe.Active && recipe.UserId == user.Id);

            if (filters.Difficulties.Any())
                // recipe.Difficulty.HasValue elimina as receitas que não tem dificuldade
                query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));

            if (filters.CookingTimes.Any())
                query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));

            if (filters.DishTypes.Any())
                query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));

            if (filters.RecipeTitle_Ingredient.NotEmpty())
                query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient)
                || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));

            return await query.ToListAsync();
        }

        async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
            => await GetFullRecipeById(user, recipeId).AsNoTracking().FirstOrDefaultAsync();

        public async Task<bool> ExistActiveRecipeWithId(User user, long recipeId)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .AnyAsync(recipe => recipe.Active && recipe.UserId == user.Id && recipe.Id == recipeId);
        }



        private IQueryable<Recipe> GetFullRecipeById(User user, long recipeId)
        {
            return _dbContext.Recipes
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.DishTypes)
                .Include(recipe => recipe.Instructions)
                .Where(recipe => recipe.Active && recipe.UserId == user.Id && recipe.Id == recipeId);
        }
    }
}
