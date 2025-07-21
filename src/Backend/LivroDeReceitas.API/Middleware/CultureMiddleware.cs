using LivroDeReceitas.Domain.Extensions;
using System.Globalization;

namespace LivroDeReceitas.API.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next; // Armazena o próximo middleware/controller... para chamar depois
        }

        public async Task Invoke(HttpContext context)
        {
            var culturesThatSpeakPortuguese = CultureInfo.GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures)
                .Where(culture => culture.TwoLetterISOLanguageName == "pt").ToList();
            var culturesThatSpeakSpanish = CultureInfo.GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures)
                .Where(culture => culture.TwoLetterISOLanguageName == "es").ToList();

            // Obtém o primeiro valor do cabeçalho Accept-Language (ex: "pt-BR", "en-US") da requisição HTTP
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var culture = new CultureInfo("en"); // Define a cultura padrão ("en") caso nenhuma cultura válida seja encontrada

            if (string.IsNullOrWhiteSpace(requestedCulture).IsFalse()
                && culturesThatSpeakPortuguese.Exists(c => c.Name.Equals(requestedCulture)))
            {
                culture = new CultureInfo("pt-BR");
            }
            else if (culturesThatSpeakSpanish.Exists(c => c.Name.Equals(requestedCulture)))
            {
                culture = new CultureInfo("es");
            }

            CultureInfo.CurrentCulture = culture; // Define a cultura atual para formatação de números, datas, moedas etc.
            CultureInfo.CurrentUICulture = culture; // Define a cultura usada para localizar recursos (como arquivos .resx) com base no idioma/região

            await _next(context); // chama esse próximo e mantém a execução fluindo corretamente.
        }
    }
}
