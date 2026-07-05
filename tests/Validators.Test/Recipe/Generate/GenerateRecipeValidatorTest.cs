using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Generate;
using LivroDeReceitas.Domain.ValueObjects;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Test.Recipe.Generate
{
    public class GenerateRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new GenerateRecipeValidator();

            var request = RequestGenerateRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_MoreMaximumIngredient()
        {
            var validator = new GenerateRecipeValidator();

            var request = RequestGenerateRecipeJsonBuilder.Build(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);
        }

        [Fact]
        public void Error_DuplicatedIngredient()
        {
            var validator = new GenerateRecipeValidator();

            var request = RequestGenerateRecipeJsonBuilder.Build(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
            request.Ingredients.Add(request.Ingredients.First());

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testes de Unidade")]
        public void Error_EmptyIngredient(string ingredient)
        {
            var validator = new GenerateRecipeValidator();

            var request = RequestGenerateRecipeJsonBuilder.Build(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
            request.Ingredients.Add(ingredient);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_EMPTY);
        }

        [Fact]
        public void Error_IngredientNotFollowingPattern()
        {
            var validator = new GenerateRecipeValidator();

            var request = RequestGenerateRecipeJsonBuilder.Build(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
            request.Ingredients.Add("This is an invalid ingredient because is too long");

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
        }
    }
}
