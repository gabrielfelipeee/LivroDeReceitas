using LivroDeReceitas.Domain.Dtos;
using LivroDeReceitas.Domain.Enums;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Services.OpenAI;
using OpenAI.Chat;

namespace LivroDeReceitas.Infrastructure.Services.OpenAI;

public class ChatGPTService : IGenerateRecipeAI
{
    private readonly ChatClient _chatClient;
    public ChatGPTService(ChatClient chatClient) => _chatClient = chatClient;

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        // Cria a lista de mensagens que será enviada ao modelo
        var messages = new List<ChatMessage>
        {
            // Mensagem de sistema com instruções iniciais (prompt)
            new SystemChatMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE),
            
            // Mensagem do usuário contendo os ingredientes separados por ';'
            new UserChatMessage(string.Join(";", ingredients))
        };

        // Executa a chamada ao modelo de IA e aguarda a resposta
        var completion = await _chatClient.CompleteChatAsync(messages);

        // Processa o texto retornado pela IA
        var responseList = completion.Value.Content[0].Text
            .Split("\n") // Divide por linhas
            .Where(response => response.Trim().Equals(string.Empty).IsFalse()) // Remove linhas vazias
            .Select(item => item.Replace("[", "").Replace("]", "")) // Remove colchetes
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = Enum.Parse<CookingTime>(responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = [.. responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            })]
        };
    }
}
