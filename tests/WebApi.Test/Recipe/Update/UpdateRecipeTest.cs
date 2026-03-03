using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Update
{
    public class UpdateRecipeTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "recipe";

        private readonly Guid _userIdentifier;
        private readonly string _recipeId;
        public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(method: $"{METHOD}/{_recipeId}", request: request, token: token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_TitleEmpty(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var response = await DoPut(method: $"{METHOD}/{_recipeId}", request: request, token: token, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

            // erros da ResponseErrorJson
            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }


    }
}
