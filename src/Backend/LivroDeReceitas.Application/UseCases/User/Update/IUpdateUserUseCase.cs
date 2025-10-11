using LivroDeReceitas.Comunication.Requests;

namespace LivroDeReceitas.Application.UseCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        public Task Execute(RequestUpdateUserJson request);
    }
}
