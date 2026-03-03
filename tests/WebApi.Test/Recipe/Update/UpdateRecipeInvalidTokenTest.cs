using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.Update
{
    public class UpdateRecipeInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "recipe";

        private readonly string _recipeId;
        public UpdateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _recipeId = factory.GetRecipeId();  
        }

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut($"{METHOD}/{_recipeId}", request, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut($"{METHOD}/{_recipeId}", request, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut($"{METHOD}/{_recipeId}", request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
