using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Services.ServiceBus
{
    public interface IDeleteUserQueue
    {
        Task SendMessage(User user);
    }
}
