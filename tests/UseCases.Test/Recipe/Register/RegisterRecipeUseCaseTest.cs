using CommomTestUtilities.BlobStorage;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Register;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success_WithoutImage()
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestRegisterRecipeFormDataBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldSatisfyAllConditions(recipe =>
            {
                recipe.ShouldNotBeNull();
                recipe.Id.ShouldNotBeNullOrWhiteSpace();
                recipe.Title.ShouldBe(request.Title);
            });
        }

        [Theory]
        [ClassData(typeof(ImageTypesInlineData))]
        public async Task Success_WithImage(IFormFile file)
        {
            (var user, _) = UserEntityBuilder.Build();

            var request = RequestRegisterRecipeFormDataBuilder.Build(file);

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

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }

        [Fact]
        public async Task Error_InvalidFile()
        {
            (var user, _) = UserEntityBuilder.Build();

            var textFile = FormFileBuilder.Txt();

            var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

            var useCase = CreateUseCase(user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
        }

        private static RegisterRecipeUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var blobStorage = new BlobStorageServiceBuilder().Build();

            return new RegisterRecipeUseCase(writeOnlyRepository, blobStorage, unitOfWork, mapper, loggedUser);
        }
    }
}
