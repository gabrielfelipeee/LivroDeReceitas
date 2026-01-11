using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Enums;
using Bogus;

namespace CommomTestUtilities.Requests
{
    public class RequestRecipeJsonBuilder
    {
        public static RequestRecipeJson Build()
        {
            return new Faker<RequestRecipeJson>()
                .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTime, faker => faker.PickRandom<CookingTime>())
                .RuleFor(recipe => recipe.Difficulty, faker => faker.PickRandom<Difficulty>());
            
            // PickRandom vai escolher aleatoriamente um dos valores definidos no enum
        }
    }
}
