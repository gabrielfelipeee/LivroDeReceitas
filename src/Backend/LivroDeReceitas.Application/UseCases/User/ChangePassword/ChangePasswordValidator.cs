using FluentValidation;
using LivroDeReceitas.Application.SharedValidators;
using LivroDeReceitas.Comunication.Requests;

namespace LivroDeReceitas.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        }
    }
}
