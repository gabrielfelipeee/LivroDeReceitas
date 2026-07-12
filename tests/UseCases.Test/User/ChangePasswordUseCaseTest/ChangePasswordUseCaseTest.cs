using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.User.ChangePassword;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.ChangePasswordUseCaseTest
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var userEntity, var password) = UserEntityBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(userEntity);

            Func<Task> act = async () => { await useCase.Execute(request); };
            act.ShouldNotThrow();

            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            passwordEncrypter.IsValid(password: request.NewPassword, passwordHash: userEntity.Password).ShouldBe(true);
        }

        [Fact]
        public async Task Error_NewPasswordEmpty()
        {
            (var userEntity, var password) = UserEntityBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                Password = password,
                NewPassword = string.Empty
            };

            var useCase = CreateUseCase(userEntity);

            // Verifica se a exception é do tipo ErrorOnValidationException
            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);

            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            passwordEncrypter.IsValid(password: password, passwordHash: userEntity.Password).ShouldBe(true); // Para garantir que a senha atual não foi alterada
        }

        [Fact]
        public async Task Error_CurrentPasswordDifferent()
        {
            (var userEntity, var password) = UserEntityBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(userEntity);

            // Verifica se a exception é do tipo ErrorOnValidationException
            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);

            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            passwordEncrypter.IsValid(password: password, passwordHash: userEntity.Password).ShouldBe(true); // Para garantir que a senha atual não foi alterada
        }

        private static ChangePasswordUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User userEntity)
        {
            var loggedUser = LoggedUserBuilder.Build(userEntity);
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(userEntity).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncrypter);
        }
    }
}
