using FluentValidation;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Exceptions;

namespace LivroDeReceitas.Application.UseCases.Recipe
{
    public class RecipeValidator : AbstractValidator<RequestRecipeJson>
    {
        public RecipeValidator()
        {
            RuleFor(recipe => recipe.Title)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.RECIPE_TITLE_EMPTY);
            RuleFor(recipe => recipe.CookingTime)
                .IsInEnum() // Verifica se o número recebido está no enum
                .WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
            RuleFor(recipe => recipe.Difficulty)
                .IsInEnum()
                .WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        }
    }
}
