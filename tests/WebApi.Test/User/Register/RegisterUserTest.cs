using CommomTestUtilities.Requests;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : LivroDeReceitasClassFixture
    {
        private readonly string METHOD = "user";

        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }


        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            // Envia a requisição HTTP POST para o endpoint "User"
            var response = await DoPost(METHOD, request);

            // Verifica se o status HTTP retornado é 201 Created
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // Lê o corpo da resposta como stream
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Faz o parsing do corpo da resposta para um JsonDocument
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // Verifica se a propriedade "name" existe e corresponde ao valor enviado na requisição
            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(name =>
            {
                name.ShouldNotBeNullOrWhiteSpace();
                name.ShouldBe(request.Name);
            });
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrWhiteSpace();
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_NameEmpty(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(METHOD, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));


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
