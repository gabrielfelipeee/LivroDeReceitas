using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
