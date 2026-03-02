using CommomTestUtilities.IdEncryption;
using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.Delete
{
    public class DeleteRecipeInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "recipe";
        public DeleteRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        { }

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var recipeId = IdEncripterBuilder.Build().Encode(1);

            var response = await DoDelete($"{METHOD}/{recipeId}", token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var recipeId = IdEncripterBuilder.Build().Encode(1);

            var response = await DoDelete($"{METHOD}/{recipeId}", token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var recipeId = IdEncripterBuilder.Build().Encode(1);

            var response = await DoDelete($"{METHOD}/{recipeId}", token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
