using CommomTestUtilities.Tokens;
using LivroDeReceitas.Comunication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "user/change-password";
        public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        { }

        [Fact]
        public async Task Error_TokenInvalid()
        {
            var request = new RequestChangePasswordJson();

            var response = await DoPut(METHOD, request, token: "invalidtoken");
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_WithoutToken()
        {
            var request = new RequestChangePasswordJson();

            var response = await DoPut(METHOD, request, token: string.Empty);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_TokenWithUserNotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = new RequestChangePasswordJson();

            var response = await DoPut(METHOD, request, token);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
