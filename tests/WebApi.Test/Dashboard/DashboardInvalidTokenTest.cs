using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Dashboard
{
    public class DashboardInvalidTokenTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "dashboard";
        public DashboardInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        { }

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
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
