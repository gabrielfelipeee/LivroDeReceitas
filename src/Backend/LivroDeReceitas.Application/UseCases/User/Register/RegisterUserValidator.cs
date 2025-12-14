using FluentValidation;
using LivroDeReceitas.Application.SharedValidators;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;

namespace LivroDeReceitas.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY);

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());

            // Para evitar que o a mensagem de email inválido seja enviado quando o email for nulo ou vazio
            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email)
                    .EmailAddress()
                    .WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
