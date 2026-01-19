using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Enums;
using Bogus;

namespace CommomTestUtilities.Requests
{
    public class RequestFilterRecipeJsonBuilder
    {
        public static RequestFilterRecipeJson Build()
        {
            return new Faker<RequestFilterRecipeJson>()
                .RuleFor(recipe => recipe.RecipeTitle_Ingredient, faker => faker.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTimes, faker => faker.Make(1, () => faker.PickRandom<CookingTime>()))
                .RuleFor(recipe => recipe.DishTypes, faker => faker.Make(1, () => faker.PickRandom<DishType>()))
                .RuleFor(recipe => recipe.Difficulties, faker => faker.Make(1, () => faker.PickRandom<Difficulty>()));
        }
    }
}
