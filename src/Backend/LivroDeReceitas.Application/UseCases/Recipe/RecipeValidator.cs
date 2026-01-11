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
            RuleFor(recipe => recipe.Ingredients.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
            RuleFor(recipe => recipe.Instructions.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);

            RuleForEach(recipe => recipe.DishTypes) // Verifica cada elemento da lista
                .IsInEnum()
                .WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
            RuleForEach(recipe => recipe.Ingredients)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);
            RuleForEach(recipe => recipe.Instructions)
                .ChildRules(instructionRule => // Adiciona uma regra para cada elemento da lista
                {
                    instructionRule.RuleFor(instruction => instruction.Step)
                        .GreaterThan(0)
                        .WithMessage(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP);
                    instructionRule.RuleFor(instruction => instruction.Text)
                        .NotEmpty()
                        .WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY)
                        .MinimumLength(2000)
                        .WithMessage(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
                });
            RuleFor(recipe => recipe.Instructions)
                .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count) // Verifica se cada instrução tem o Step único: Must deve retornar true
                .WithMessage(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
        }
    }
}
