using LivroDeReceitas.Comunication.Requests;

namespace LivroDeReceitas.Application.UseCases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        public Task Execute(RequestChangePasswordJson request);
    }
}
