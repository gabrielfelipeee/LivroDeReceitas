using CommomTestUtilities.Dtos;
using CommomTestUtilities.OpenAI;
using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Generate;
using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.ValueObjects;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Generate
{
    public class GenerateRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var dto = GenerateRecipeDtoBuilder.Build();

            var request = RequestGenerateRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(dto);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Title.ShouldBe(dto.Title);
            result.CookingTime.ShouldBe((LivroDeReceitas.Comunication.Enums.CookingTime)dto.CookingTime);
            result.Difficulty.ShouldBe((LivroDeReceitas.Comunication.Enums.Difficulty)dto.Difficulty);
        }

        [Fact]
        public async Task Error_DuplicatedIngredients()
        {
            var dto = GenerateRecipeDtoBuilder.Build();

            var request = RequestGenerateRecipeJsonBuilder.Build(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
            request.Ingredients.Add(request.Ingredients.First());

            var useCase = CreateUseCase(dto);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(async () => await useCase.Execute(request));
            exception.GetErrorMessages().Count.ShouldBe(1);
            exception.GetErrorMessages().ShouldContain(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
        }

        private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
        {
            var generator = GenerateRecipeAIBuilder.Build(dto);

            return new GenerateRecipeUseCase(generator);
        }
    }
}
