using LivroDeReceitas.Comunication.Requests;

namespace LivroDeReceitas.Application.UseCases.Recipe.Update
{
    public interface IUpdateRecipeUseCase
    {
        Task Execute(long recipeId, RequestRecipeJson request);
    }
}
