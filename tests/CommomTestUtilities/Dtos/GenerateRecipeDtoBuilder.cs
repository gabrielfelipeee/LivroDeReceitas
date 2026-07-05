using Bogus;
using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.ValueObjects;

namespace CommomTestUtilities.Dtos
{
    public class GenerateRecipeDtoBuilder
    {
        public static GeneratedRecipeDto Build()
        {
            return new Faker<GeneratedRecipeDto>()
                 .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
                 .RuleFor(recipe => recipe.Ingredients, faker => faker.Make(LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE, () => faker.Commerce.ProductName()))
                 .RuleFor(recipe => recipe.CookingTime, faker => faker.PickRandom<CookingTime>())
                 .RuleFor(recipe => recipe.Difficulty, faker => Difficulty.Low)
                 .RuleFor(recipe => recipe.Instructions, faker => faker.Make(1, () => new GeneratedInstructionDto
                 {
                     Step = 1,
                     Text = faker.Lorem.Paragraph()
                 }));
        }
    }
}
