using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe.Filter;
using LivroDeReceitas.Comunication.Enums;
using LivroDeReceitas.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_InvalidCookingTime()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((CookingTime)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_InvalidDifficulty()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.Difficulties.Add((Difficulty)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_InvalidDishTypes()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }
    }
}
