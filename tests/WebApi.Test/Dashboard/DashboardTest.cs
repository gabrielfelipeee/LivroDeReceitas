using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Dashboard
{
    public class DashboardTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "dashboard";

        private readonly Guid _userIdentifier;
        public DashboardTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token: token);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("recipes").GetArrayLength().ShouldBeGreaterThan(0);
        }
    }
}
