using CommomTestUtilities.IdEncryption;
using CommomTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.GetById;

public class GetRecipeByIdInvalidTokenTest : LivroDeReceitasClassFixture
{
    private readonly string METHOD = "recipe";
    public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Error_TokenInvalid()
    {
        var recipeId = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{recipeId}", token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WithoutToken()
    {
        var recipeId = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{recipeId}", token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_TokenWithUserNotFound()
    {
        var recipeId = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet($"{METHOD}/{recipeId}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
