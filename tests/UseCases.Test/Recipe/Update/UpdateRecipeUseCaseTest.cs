using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Update;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Update
{
    public class UpdateRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserEntityBuilder.Build();

            var existingRecipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user, existingRecipe);

            Func<Task> act = async () => { await useCase.Execute(existingRecipe.Id, request); };
            act.ShouldNotThrow();
        }

        [Fact]
        public async Task Error_RecipeNotFound()
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(recipeId: 100, request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        [Fact]
        public async Task Error_TitleEmpty()
        {
            (var user, _) = UserEntityBuilder.Build();

            var existingRecipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user, existingRecipe);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(existingRecipe.Id, request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }

        private static UpdateRecipeUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user, LivroDeReceitas.Domain.Entities.Recipe? existingRecipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var recipeUpdateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, existingRecipe).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            return new UpdateRecipeUseCase(loggedUser, recipeUpdateOnlyRepository, unitOfWork, mapper);
        }
    }
}
