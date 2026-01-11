using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Enums;
using Bogus;

namespace CommomTestUtilities.Requests
{
    public class RequestRecipeJsonBuilder
    {
        public static RequestRecipeJson Build()
        {
            int step = 1;

            return new Faker<RequestRecipeJson>()
                .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTime, faker => faker.PickRandom<CookingTime>())
                .RuleFor(recipe => recipe.Difficulty, faker => faker.PickRandom<Difficulty>())
                .RuleFor(recipe => recipe.Ingredients, faker => faker.Make(3, () => faker.Commerce.ProductName()))
                .RuleFor(recipe => recipe.DishTypes, faker => faker.Make(3, () => faker.PickRandom<DishType>()))
                .RuleFor(recipe => recipe.Instructions, faker => faker.Make(3, () => new RequestInstructionJson
                {
                    Step = step++,
                    Text = faker.Lorem.Paragraph()
                }));

            // PickRandom vai escolher aleatoriamente um dos valores definidos no enum
        }
    }
}
