using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using LivroDeReceitas.Application.UseCases.Recipe.GetById;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.GetById;

public class GetRecipeByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserEntityBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var result = await useCase.Execute(recipe.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(recipe.Title);
    }

    [Fact]
    public async Task Error_RecipeNotFound()
    {
        (var user, _) = UserEntityBuilder.Build();

        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(recipeId: 100));
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    private static GetRecipeByIdUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user, LivroDeReceitas.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var mapper = MapperBuilder.Build();

        return new GetRecipeByIdUseCase(mapper, loggedUser, recipeReadOnlyRepository);
    }
}
