using CommomTestUtilities.Requests;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : LivroDeReceitasClassFixture
    {
        private readonly string method = "login";

        private readonly string _name;
        private readonly string _email;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetName();
            _email = factory.GetEmail();
            _password = factory.GetPassword();
        }


        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(method, request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(name =>
            {
                name.ShouldNotBeNull();
                name.ShouldBe(_name);
            });
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_LoginInvalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(method, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }
    }
}
