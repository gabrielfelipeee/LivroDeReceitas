using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Register;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldSatisfyAllConditions(recipe =>
            {
                recipe.ShouldNotBeNull();
                recipe.Id.ShouldNotBeNullOrWhiteSpace();
                recipe.Title.ShouldBe(request.Title);
            });
        }

        [Fact]
        public async Task Error_TitleEmpty()
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.ErrorMessages.Count.ShouldBe(1);
            exception.ErrorMessages.ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }

        private static RegisterRecipeUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            return new RegisterRecipeUseCase(writeOnlyRepository, unitOfWork, mapper, loggedUser);
        }
    }
}
