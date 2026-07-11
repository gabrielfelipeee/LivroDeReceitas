using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Register
{
    public class RegisterRecipeTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "recipe";
        private readonly Guid _userIdentifier;

        public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }


        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Envia a requisição HTTP POST para o endpoint "Recipe"
            var response = await DoPostFormData(method: METHOD, request: request, token: token);

            // Verifica se o status HTTP retornado é 201 Created
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // Lê o corpo da resposta como stream
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Faz o parsing do corpo da resposta para um JsonDocument
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // Verifica se a propriedade "title" existe e corresponde ao valor enviado na requisição
            responseData.RootElement.GetProperty("title").GetString().ShouldSatisfyAllConditions(title =>
            {
                title.ShouldNotBeNullOrWhiteSpace();
                title.ShouldBe(request.Title);
            });
            responseData.RootElement.GetProperty("id").GetString().ShouldNotBeNullOrWhiteSpace();
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_TitleEmpty(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Title = string.Empty;

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPostFormData(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));


            // erros da ResponseErrorJson
            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }
    }
}
