using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Recipe.Register
{
    public interface IRegisterRecipeUseCase
    {
        public Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
    }
}


