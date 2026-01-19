using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter
{
    public class FilterRecipeTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "recipe/filter";
        private readonly Guid _userIdentifier;

        private string _recipeTitle;
        private LivroDeReceitas.Domain.Enums.Difficulty _recipeDifficultyLevel;
        private LivroDeReceitas.Domain.Enums.CookingTime _recipeCookingTime;
        private IList<LivroDeReceitas.Domain.Enums.DishType> _recipeDishTypes;

        public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();

            _recipeTitle = factory.GetRecipeTitle();
            _recipeDifficultyLevel = factory.GetRecipeDifficulty();
            _recipeCookingTime = factory.GetRecipeCookingTime();
            _recipeDishTypes = factory.GetRecipeDishTypes();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestFilterRecipeJson
            {
                RecipeTitle_Ingredient = _recipeTitle,
                CookingTimes = [(LivroDeReceitas.Comunication.Enums.CookingTime)_recipeCookingTime],
                Difficulties = [(LivroDeReceitas.Comunication.Enums.Difficulty)_recipeDifficultyLevel],
                DishTypes = _recipeDishTypes.Select(dishType => (LivroDeReceitas.Comunication.Enums.DishType)dishType).ToList()
            };

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("recipes").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(recipes =>
                {
                    recipes.ShouldNotBeNull();
                });
        }

        [Fact]
        public async Task Success_NoContent()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.RecipeTitle_Ingredient = "recipeDontExist";

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CookingTimeInvalid(string culture)
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((LivroDeReceitas.Comunication.Enums.CookingTime)1000);

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }
    }
}
