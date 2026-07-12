using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Security.Cryptography;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncripter _passwordEncripter;
        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncripter passwordEncripter)
        {
            _loggedUser = loggedUser;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
            _passwordEncripter = passwordEncripter;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request, loggedUser);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);
            user.Password = _passwordEncripter.Encrypt(request.NewPassword);

            _userUpdateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
        {
            var result = new ChangePasswordValidator().Validate(request);

            if (_passwordEncripter.IsValid(password: request.Password, passwordHash: loggedUser.Password).IsFalse())
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

            if (result.IsValid.IsFalse())
                throw new ErrorOnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
        }
    }
}
