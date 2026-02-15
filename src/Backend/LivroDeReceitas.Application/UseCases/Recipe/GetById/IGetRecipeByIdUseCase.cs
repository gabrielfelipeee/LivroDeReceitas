using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Recipe.GetById
{
    public interface IGetRecipeByIdUseCase
    {
        Task<ResponseRecipeJson> Execute(long recipeId);
    }
}
