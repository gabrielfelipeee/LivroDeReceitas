namespace LivroDeReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeWriteOnlyRepository
    {
        public Task Add(Entities.Recipe recipe);
    }
}
