using CommomTestUtilities.Requests;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly string method = "user";

        // WebApplicationFactory configura o ambiente de teste com suporte a DI e pipeline completo da aplicação
        private readonly HttpClient _httpClient;
        public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();


        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            // Envia a requisição HTTP POST para o endpoint "User"
            var response = await _httpClient.PostAsJsonAsync(method, request);

            // Verifica se o status HTTP retornado é 201 Created
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // Lê o corpo da resposta como stream
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Faz o parsing do corpo da resposta para um JsonDocument
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // Verifica se a propriedade "name" existe e corresponde ao valor enviado na requisição
            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(name =>
            {
                name.ShouldNotBeNull();
                name.ShouldBe(request.Name);
            });
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_NameEmpty(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
            var response = await _httpClient.PostAsJsonAsync(method, request);

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
