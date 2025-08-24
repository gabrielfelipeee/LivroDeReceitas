using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "user";

        public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory){}
        

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var response = await DoGet(METHOD, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var response = await DoGet(METHOD, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            // Gera um token para um usuário que não existe
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
