using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Recipe.Filter
{
    public interface IFilterRecipeUseCase
    {
        public Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request);
    }
}
