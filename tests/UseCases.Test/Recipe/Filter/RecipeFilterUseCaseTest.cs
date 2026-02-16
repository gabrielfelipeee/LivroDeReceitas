using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Filter;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Filter
{
    public class RecipeFilterUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.ShouldNotBeEmpty();
            result.Recipes.Count.ShouldBe(recipes.Count);
        }

        [Fact]
        public async Task Error_CookingTimeInvalid()
        {
            (var user, _) = UserEntityBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((LivroDeReceitas.Comunication.Enums.CookingTime)1000);

            var useCase = CreateUseCase(user, recipes);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        private static FilterRecipeUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user, IList<LivroDeReceitas.Domain.Entities.Recipe> recipes)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();
            var mapper = MapperBuilder.Build();

            return new FilterRecipeUseCase(mapper, loggedUser, recipeReadOnlyRepository);
        }
    }
}
