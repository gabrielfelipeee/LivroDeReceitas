using CommomTestUtilities.BlobStorage;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using LivroDeReceitas.Application.UseCases.Dashboard;
using Shouldly;

namespace UseCases.Test.Dashboard
{
    public class DashboardUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserEntityBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execute();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.ShouldNotBeEmpty();
            result.Recipes.Count.ShouldBeGreaterThan(0);
        }


        private static DashboardUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user, IList<LivroDeReceitas.Domain.Entities.Recipe> recipes)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
            var mapper = MapperBuilder.Build();
            var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();

            return new DashboardUseCase(loggedUser, recipeReadOnlyRepository, blobStorage, mapper);
        }
    }
}
