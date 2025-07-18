using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.User.Register;
using LivroDeReceitas.Exceptions;
using Shouldly;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            var result = validator.Validate(request);

            Assert.True(result.IsValid); // Forma nativa
            result.IsValid.ShouldBeTrue(); // Espera que o resultado seja sucesso
        }

        [Fact]
        public void Error_NameEmpty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1); // Verifica se contém exatamente um único item

            // Espera que a mensagem de erro do primeiro item da lista de erros seja igual à ResourceMessagesException.NAME_EMPTY
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Error_EmailEmpty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_EmailInvalid()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "emailinvalid";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
        }

        // O teste será executado 6 vezes, com os valores 0, 1, 2, 3, 4 e 5 sendo passados como argumento para o parâmetro passwordLength
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_PasswordInvalid(int passwordLength)
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build(passwordLength);
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_MUST_BE_LONGER_THAN_6_CHARACTERS);
        }
    }
}
