using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser.Email);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);
            user.Name = request.Name;
            user.Email = request.Email;

            _userUpdateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }


        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);

            if (currentEmail.Equals(request.Email).IsFalse())
            {
                var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
                if (userExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
