using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordTest : LivroDeReceitasClassFixture
    {
        private const string METHOD = "user/change-password";

        private readonly string _password;
        private readonly string _email;
        private readonly Guid _userIdentifier;

        public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _password = factory.GetPassword();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();
        }


        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(METHOD, request, token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            response = await DoPost("login", loginRequest);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized); // Senha Antiga

            loginRequest.Password = request.NewPassword; // Senha Atual
            response = await DoPost("login", loginRequest);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_NewPasswordEmpty(string culture)
        {
            var request = new RequestChangePasswordJson
            {
                Password = _password,
                NewPassword = string.Empty
            };

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));

            responseData.RootElement.GetProperty("errors").EnumerateArray().ToList()
                .ShouldSatisfyAllConditions(errors =>
                {
                    errors.Count.ShouldBe(1);
                    errors.Single().GetString().ShouldBe(expectedMessage);
                });
        }

    }
}
