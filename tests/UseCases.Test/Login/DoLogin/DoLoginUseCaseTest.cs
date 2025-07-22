using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Login.DoLogin;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var userEntity, var password) = UserEntityBuilder.Build();

            var useCase = CreateUseCase(userEntity);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = userEntity.Email,
                Password = password
            });

            result.ShouldSatisfyAllConditions(user =>
            {
                user.ShouldNotBeNull();
                user.Name.ShouldNotBeNullOrWhiteSpace();
                user.Name.ShouldBe(userEntity.Name);
            });

        }

        [Fact]
        public async Task Error_InvalidUser()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            var exception = await Should.ThrowAsync<InvalidLoginException>(async () => await useCase.Execute(request));
            exception.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        }


        private static DoLoginUseCase CreateUseCase(UserEntity? userEntity = null)
        {
            var passwordEncrypter = PasswordEncrypterBuilder.Build();

            var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if (userEntity is not null)
                readOnlyRepositoryBuilder.GetByEmailAndPassword(userEntity);

            return new DoLoginUseCase(readOnlyRepositoryBuilder.Build(), passwordEncrypter);
        }
    }
}
