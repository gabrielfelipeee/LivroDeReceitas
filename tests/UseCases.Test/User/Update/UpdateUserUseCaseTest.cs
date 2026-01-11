using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.User.Update;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;
using System.Security.AccessControl;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var userEntity, _) = UserEntityBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(userEntity);

            Func<Task> act = async () => await useCase.Execute(request);

            await Should.NotThrowAsync(act);
            userEntity.Name.ShouldBe(request.Name);
            userEntity.Email.ShouldBe(request.Email);
        }

        [Fact]
        public async Task Error_NameEmpty()
        {
            (var userEntity, _) = UserEntityBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(userEntity);

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(act);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.NAME_EMPTY);

            userEntity.Name.ShouldNotBe(request.Name);
            userEntity.Email.ShouldNotBe(request.Email);
        }

        [Fact]
        public async Task Error_EmailAlreadyRegistered()
        {
            (var userEntity, _) = UserEntityBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(userEntity, request.Email);

            Func<Task> act = async () => await useCase.Execute(request);
            var exception = await Should.ThrowAsync<ErrorOnValidationException>(act);
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);

            userEntity.Name.ShouldNotBe(request.Name);
            userEntity.Email.ShouldNotBe(request.Email);
        }

        private static UpdateUserUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User userEntity, string? email = null)
        {
            var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if (email.NotEmpty())
                readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

            var loggedUser = LoggedUserBuilder.Build(userEntity);
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(userEntity).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UpdateUserUseCase(loggedUser, userUpdateRepository, readOnlyRepositoryBuilder.Build(), unitOfWork);
        }
    }
}
