using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
