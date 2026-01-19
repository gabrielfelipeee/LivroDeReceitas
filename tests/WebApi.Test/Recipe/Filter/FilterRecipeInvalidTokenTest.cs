using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.Recipe.Filter
{
    public class FilterRecipeInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "recipe/filter";

        public FilterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        { }

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, token: "invalidToken");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, token: string.Empty);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
