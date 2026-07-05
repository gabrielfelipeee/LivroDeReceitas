using Bogus;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.ValueObjects;

namespace CommomTestUtilities.Requests
{
    public class RequestGenerateRecipeJsonBuilder
    {
        public static RequestGenerateRecipeJson Build(int count = LivroDeReceitasRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE)
        {
            return new Faker<RequestGenerateRecipeJson>()
                .RuleFor(recipe => recipe.Ingredients, faker => faker.Make(count, () => faker.Commerce.ProductName()));
        }
    }
}
