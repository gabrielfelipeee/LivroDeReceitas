using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.Services.OpenAI;
using Moq;

namespace CommomTestUtilities.OpenAI
{
    public class GenerateRecipeAIBuilder
    {
        public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
        {
            var mock = new Mock<IGenerateRecipeAI>();

            mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

            return mock.Object;
        }
    }
}

