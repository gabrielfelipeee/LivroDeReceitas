using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Application.UseCases.User.Register;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;


namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.ShouldSatisfyAllConditions(user =>
            {
                user.ShouldNotBeNull();
                user.Tokens.ShouldNotBeNull();
                user.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
                user.Name.ShouldBe(request.Name);
            });
        }

        [Fact]
        public async Task Error_EmailAlreadyRegistered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            // Verifica se a exception é do tipo ErrorOnValidationException
            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
        }

        [Fact]
        public async Task Error_NameEmpty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NAME_EMPTY);
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if (email.NotEmpty())
                readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var passwordEncrypter = PasswordEncrypterBuilder.Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            return new RegisterUserUseCase(readOnlyRepositoryBuilder.Build(), writeOnlyRepository, unitOfWork, passwordEncrypter, accessTokenGenerator, mapper);
        }
    }
}
