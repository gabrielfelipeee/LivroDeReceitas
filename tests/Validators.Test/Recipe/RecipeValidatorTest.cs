using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.Recipe;
using LivroDeReceitas.Comunication.Enums;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testes de Unidade")]
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

        // Listas

        [Fact]
        public void Success_DishTypesEmpty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_InvalidDishTypes()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_EmptyIngredients()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testes de Unidade")]
        public void Error_EmptyValueIngredients(string ingredient)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingredient);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_EMPTY);
        }

        [Fact]
        public void Error_SameStepInstructions()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
        }

        [Fact]
        public void Error_NegativeStepInstructions()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = -1;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testes de Unidade")]
        public void Error_EmptyValueInstructions(string instruction)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = instruction;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EMPTY);
        }

        [Fact]
        public void Error_InstructionsTooLong()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            var validator = new RecipeValidator();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        }
    }
}
