using LivroDeReceitas.Domain.Dtos;

namespace LivroDeReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipeDto filters);
        Task<Entities.Recipe?> GetById(Entities.User user, long recipeId);
        Task<IList<Entities.Recipe>> GetForDashboard(Entities.User user);
        public Task<bool> ExistActiveRecipeWithId(Entities.User user, long recipeId);
    }
}
