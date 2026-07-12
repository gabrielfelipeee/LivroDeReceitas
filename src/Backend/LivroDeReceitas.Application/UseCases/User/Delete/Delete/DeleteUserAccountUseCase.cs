using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Services.Storage;

namespace LivroDeReceitas.Application.UseCases.User.Delete.Delete
{
    public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
    {
        private readonly IUserDeleteOnlyRepository _userDeleteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;

        public DeleteUserAccountUseCase(
            IUserDeleteOnlyRepository userDeleteOnlyRepository,
            IUnitOfWork unitOfWork,
            IBlobStorageService blobStorageService)
        {
            _userDeleteOnlyRepository = userDeleteOnlyRepository;
            _unitOfWork = unitOfWork;
            _blobStorageService = blobStorageService;
        }

        public async Task Execute(Guid userIdentifier)
        {
            await _blobStorageService.DeleteContainer(userIdentifier);

            await _userDeleteOnlyRepository.DeleteAccount(userIdentifier);
            await _unitOfWork.Commit();
        }
    }
}
