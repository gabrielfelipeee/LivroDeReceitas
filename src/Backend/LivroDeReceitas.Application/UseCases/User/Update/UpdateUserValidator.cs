using FluentValidation;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Exceptions;

namespace LivroDeReceitas.Application.UseCases.User.Update
{
    public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
    {
        public UpdateUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY);

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            // Para evitar que o a mensagem de email inválido seja enviado quando o email for nulo ou vazio
            When(user => string.IsNullOrWhiteSpace(user.Email).IsFalse(), () =>
            {
                RuleFor(user => user.Email)
                    .EmailAddress()
                    .WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
