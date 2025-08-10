using System.Diagnostics.CodeAnalysis;

namespace LivroDeReceitas.Domain.Extensions
{
    public static class StringExtension
    {
        // O atributo [NotNullWhen(true)] informa ao compilador que, se o retorno for true, a variável 'value' não será nula.
        public static bool NotEmpty([NotNullWhen(true)] this string? value) => string.IsNullOrWhiteSpace(value).IsFalse();
    }
}
