using CommomTestUtilities.BlobStorage;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Image;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image
{
    public class AddUpdateImageCoverUseCaseTest
    {
        [Theory]
        [ClassData(typeof(ImageTypesInlineData))]
        public async Task Success(IFormFile file)
        {
            (var user, _) = UserEntityBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => { await useCase.Execute(recipe.Id, file); };
            act.ShouldNotThrow();
        }

        [Theory]
        [ClassData(typeof(ImageTypesInlineData))]
        public async Task Success_RecipeDidNotHaveImage(IFormFile file)
        {
            (var user, _) = UserEntityBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            recipe.ImageIdentifier = null;

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => { await useCase.Execute(recipe.Id, file); };
            act.ShouldNotThrow();
        }

        [Theory]
        [ClassData(typeof(ImageTypesInlineData))]
        public async Task Error_RecipeNotFound(IFormFile file)
        {
            (var user, _) = UserEntityBuilder.Build();

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(recipeId: 100, file));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        [Fact]
        public async Task Error_File_Is_Txt()
        {
            (var user, _) = UserEntityBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            var file = FormFileBuilder.Txt();

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(recipe.Id, file));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
        }


        private static AddUpdateImageCoverUseCase CreateUseCase(
            LivroDeReceitas.Domain.Entities.User user,
            LivroDeReceitas.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var blobStorage = new BlobStorageServiceBuilder().Build();

            return new AddUpdateImageCoverUseCase(loggedUser, blobStorage, updateOnlyRepository, unitOfWork);
        }
    }
}