using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Dashboard
{
    public interface IDashboardUseCase
    {
        Task<ResponseRecipesJson> Execute();
    }
}
