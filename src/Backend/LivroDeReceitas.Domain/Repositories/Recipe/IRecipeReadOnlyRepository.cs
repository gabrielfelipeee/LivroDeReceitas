using LivroDeReceitas.Domain.Dtos;

namespace LivroDeReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipeDto filters);
        Task<Entities.Recipe?> GetById(Entities.User user, long recipeId);
        public Task<bool> ExistActiveRecipeWithId(Entities.User user, long recipeId);
    }
}
