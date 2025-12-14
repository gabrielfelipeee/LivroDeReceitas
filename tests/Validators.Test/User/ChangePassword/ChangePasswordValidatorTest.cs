using CommomTestUtilities.Requests;
using LivroDeReceitas.Application.UseCases.User.ChangePassword;
using LivroDeReceitas.Exceptions;
using Shouldly;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        // O teste será executado 6 vezes, com os valores 1, 2, 3, 4 e 5 sendo passados como argumento para o parâmetro passwordLength
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_PasswordInvalid(int passwordLength)
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build(passwordLength);
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.INVALID_PASSWORD);
        }

        [Fact]
        public void Error_PasswordEmpty()
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
        }
    }
}
