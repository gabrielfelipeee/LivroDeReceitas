using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivroDeReceitas.API.Converters
{
    public partial class StringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Lê a string do JSON e aplica Trim() para remover espaços do início e do fim
            var value = reader.GetString()?.Trim();

            if (value is null)
                return null;

            // Usa uma expressão regular para substituir múltiplos espaços em branco por um único espaço
            // Ex: "  isso   é   um   teste  " => "isso é um teste"
            return RemoveExtraWhiteSpaces().Replace(value, " ");
        }

        // Método chamado automaticamente ao serializar uma string para JSON
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>  writer.WriteStringValue(value);
        

        // Define uma expressão regular
        // \s+ => corresponde a 1 ou mais caracteres de espaço em branco
        [GeneratedRegex(@"\s+")]
        private static partial Regex RemoveExtraWhiteSpaces();
    }
}
