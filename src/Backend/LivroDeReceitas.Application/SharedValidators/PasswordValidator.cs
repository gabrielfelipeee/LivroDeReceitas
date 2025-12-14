using FluentValidation;
using FluentValidation.Validators;
using LivroDeReceitas.Exceptions;

namespace LivroDeReceitas.Application.SharedValidators
{
    // Validador de senha genérico que pode ser usado em qualquer classe T
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        // Nome do validador (usado internamente pelo FluentValidation)
        public override string Name => "PasswordValidator";

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);
                return false;
            }

            if (password.Length < 6)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.INVALID_PASSWORD);
                return false;
            }
            return true;
        }

        // Define o template padrão da mensagem retornada pelo FluentValidation
        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
    }
}
