using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Application.UseCases.Login.DoLogin;
using LivroDeReceitas.Comunication.Requests;
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
                user.Tokens.ShouldNotBeNull();
                user.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
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


        private static DoLoginUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User? userEntity = null)
        {
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if (userEntity is not null)
                readOnlyRepositoryBuilder.GetByEmail(userEntity);

            return new DoLoginUseCase(readOnlyRepositoryBuilder.Build(), passwordEncrypter, accessTokenGenerator);
        }
    }
}
