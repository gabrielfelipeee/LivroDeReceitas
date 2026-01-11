using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe;
using LivroDeReceitas.Comunication.Enums;
using LivroDeReceitas.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_CookingTimeNull()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_DifficultyNull()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_InvalidCookingTime()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (CookingTime)1000;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_InvalidDifficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (Difficulty)1000;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("")]
        public void Error_EmptyTitle(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        }
    }
}
