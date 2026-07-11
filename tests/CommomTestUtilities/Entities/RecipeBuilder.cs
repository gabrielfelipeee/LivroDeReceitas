using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Enums;
using Bogus;

namespace CommomTestUtilities.Entities
{
    public class RecipeBuilder
    {
        public static IList<Recipe> Collection(User user, uint count = 2)
        {
            var list = new List<Recipe>();

            if (count == 0)
                count = 1;

            var recipeId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeRecipe = Build(user);
                fakeRecipe.Id = recipeId++;

                list.Add(fakeRecipe);
            }

            return list;
        }

        // Gera uma receita
        public static Recipe Build(User user)
        {
            return new Faker<Recipe>()
                .RuleFor(recipe => recipe.Id, _ => 1)
                .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTime, faker => faker.PickRandom<CookingTime>())
                .RuleFor(recipe => recipe.Difficulty, faker => faker.PickRandom<Difficulty>())
                .RuleFor(recipe => recipe.Ingredients, faker => faker.Make(1, () => new Ingredient
                {
                    Id = 1,
                    Item = faker.Commerce.ProductName()
                }))
                .RuleFor(recipe => recipe.Instructions, faker => faker.Make(1, () => new Instruction
                {
                    Id = 1,
                    Step = 1,
                    Text = faker.Lorem.Paragraph()
                }))
                .RuleFor(recipe => recipe.DishTypes, faker => faker.Make(1, () => new LivroDeReceitas.Domain.Entities.DishType
                {
                    Id = 1,
                    Type = faker.PickRandom<LivroDeReceitas.Domain.Enums.DishType>()
                }))
                .RuleFor(recipe => recipe.ImageIdentifier, _ => $"{Guid.NewGuid()}.png")
                .RuleFor(recipe => recipe.UserId, _ => user.Id);
        }
    }
}
