using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Services.OpenAI;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Generate;

public class GenerateRecipeUseCase : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAI _generator;
    public GenerateRecipeUseCase(IGenerateRecipeAI generator) => _generator = generator;

    public async Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        Validate(request);

        var response = await _generator.Generate(request.Ingredients);

        return new ResponseGenerateRecipeJson()
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            CookingTime = (Comunication.Enums.CookingTime)response.CookingTime,
            Instructions = [.. response.Instructions.Select(instruction => new ResponseGeneratedInstructionJson
            {
                Step = instruction.Step,
                Text = instruction.Text,
            })],
            Difficulty = Comunication.Enums.Difficulty.Low
        };
    }


    private static void Validate(RequestGenerateRecipeJson request)
    {
        var validator = new GenerateRecipeValidator();

        var result = validator.Validate(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
    }
}
