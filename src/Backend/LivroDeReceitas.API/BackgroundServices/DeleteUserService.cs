using Azure.Messaging.ServiceBus;
using LivroDeReceitas.Application.UseCases.User.Delete.Delete;
using LivroDeReceitas.Infrastructure.Services.ServiceBus;

namespace LivroDeReceitas.API.BackgroundServices
{
    public class DeleteUserService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public DeleteUserService(IServiceProvider serviceProvider, DeleteUserProcessor processor)
        {
            _serviceProvider = serviceProvider;
            _serviceBusProcessor = processor.GetProcessor();
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync; // Executa a função ProcessMessageAsync sempre que receber uma mensagem

            _serviceBusProcessor.ProcessErrorAsync += ExceptionReceivedHandler;// Executa a função ExceptionReceivedHandler ao ocorrer um erro

            await _serviceBusProcessor.StartProcessingAsync(stoppingToken);
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
        {
            var scope = _serviceProvider.CreateScope();
            var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();


            var message = eventArgs.Message.Body.ToString();
            var userIdentifier = Guid.Parse(message);

            await deleteUserUseCase.Execute(userIdentifier);
        }

        private static Task ExceptionReceivedHandler(ProcessErrorEventArgs eventArgs) => Task.CompletedTask;


        ~DeleteUserService() => Dispose();
        public override void Dispose()
        {
            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
