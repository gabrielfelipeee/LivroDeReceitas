using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.ServiceBus;

namespace LivroDeReceitas.Application.UseCases.User.Delete.Request
{
    public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IDeleteUserQueue _queue;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestDeleteUserUseCase(
            ILoggedUser loggedUser,
            IDeleteUserQueue queue,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _queue = queue;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute()
        {
            var loggedUser = await _loggedUser.User();

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            user.Active = false;
            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();

            await _queue.SendMessage(loggedUser);
        }
    }
}
