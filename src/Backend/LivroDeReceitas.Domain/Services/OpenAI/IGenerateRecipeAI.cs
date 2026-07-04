using LivroDeReceitas.Domain.Dtos;

namespace LivroDeReceitas.Domain.Services.OpenAI;

public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}
