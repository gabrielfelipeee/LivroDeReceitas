using Azure.Messaging.ServiceBus;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Services.ServiceBus;

namespace LivroDeReceitas.Infrastructure.Services.ServiceBus
{
    public class DeleteUserQueue : IDeleteUserQueue
    {
        private readonly ServiceBusSender _serviceBusSender;

        public DeleteUserQueue(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        public async Task SendMessage(User user)
        {
            await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.UserIdentifier.ToString()));
        }
    }
}
