using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request);
}
