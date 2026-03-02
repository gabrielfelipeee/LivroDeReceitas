using CommomTestUtilities.IdEncryption;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete
{
    public class DeleteRecipeTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "recipe";

        private readonly Guid _userIdentifier;
        private readonly string _recipeId;
        public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoDelete($"{METHOD}/{_recipeId}", token: token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            response = await DoGet($"{METHOD}/{_recipeId}", token: token);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_RecipeNotFound(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var recipeIdNotFound = IdEncripterBuilder.Build().Encode(100);

            var response = await DoDelete($"{METHOD}/{recipeIdNotFound}", token: token, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }
    }
}