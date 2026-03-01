using CommomTestUtilities.IdEncryption;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.GetById;

public class GetRecipeByIdTest : LivroDeReceitasClassFixture
{
    private readonly string METHOD = "recipe";

    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    private readonly string _recipeTitle;
    public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
        _recipeTitle = factory.GetRecipeTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{METHOD}/{_recipeId}", token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldBe(_recipeId);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(_recipeTitle);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_RecipeNotFound(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var recipeIdNotFound = IdEncripterBuilder.Build().Encode(100);

        var response = await DoGet($"{METHOD}/{recipeIdNotFound}", token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

        responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
            .ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.Single().GetString().ShouldBe(expectedMessage);
            });
    }

}
