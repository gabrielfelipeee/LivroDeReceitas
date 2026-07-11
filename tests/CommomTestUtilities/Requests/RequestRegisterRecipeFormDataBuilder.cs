using Bogus;
using LivroDeReceitas.Comunication.Enums;
using LivroDeReceitas.Comunication.Requests;
using Microsoft.AspNetCore.Http;

namespace CommomTestUtilities.Requests
{
    public class RequestRegisterRecipeFormDataBuilder
    {

        public static RequestRegisterRecipeFormData Build(IFormFile? file = null)
        {
            var step = 1;

            return new Faker<RequestRegisterRecipeFormData>()
                .RuleFor(recipe => recipe.Image, _ => file)
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
        }

    }
}
