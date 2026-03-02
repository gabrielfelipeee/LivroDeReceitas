using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using LivroDeReceitas.Application.UseCases.Recipe.Delete;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Delete
{
    public class DeleteRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserEntityBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => { await useCase.Execute(recipe.Id); };
            act.ShouldNotThrow();
        }

        [Fact]
        public async Task Error_RecipeNotFound()
        {
            (var user, _) = UserEntityBuilder.Build();

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(recipeId: 100));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
        }


        private static DeleteRecipeUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user, LivroDeReceitas.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder().ExistActiveRecipeWithId(user, recipe).Build();
            var recipeWriteOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteRecipeUseCase(loggedUser, recipeReadOnlyRepository, recipeWriteOnlyRepository, unitOfWork);
        }
    }
}
