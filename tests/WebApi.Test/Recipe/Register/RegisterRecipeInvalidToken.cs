using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.Register
{
    public class RegisterRecipeInvalidToken : LivroDeReceitasClassFixture
    {
        private const string METHOD = "recipe";

        public RegisterRecipeInvalidToken(CustomWebApplicationFactory factory) : base(factory)
        {}

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(METHOD, request, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(METHOD, request, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(METHOD, request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
