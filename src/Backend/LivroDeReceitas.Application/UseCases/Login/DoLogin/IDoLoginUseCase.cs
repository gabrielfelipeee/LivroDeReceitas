using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;

namespace LivroDeReceitas.Application.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
    }
}
