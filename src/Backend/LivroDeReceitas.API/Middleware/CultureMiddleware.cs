using LivroDeReceitas.Domain.Extensions;
using System.Globalization;

namespace LivroDeReceitas.API.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _portugueseCultures;
        private readonly List<string> _spanishCultures;
        private readonly string _defaultCulture;

        public CultureMiddleware(RequestDelegate next, string defaultCulture = "en")
        {
            _next = next;
            _defaultCulture = defaultCulture;

            // Pré-calcula as listas para não recriar a cada requisição
            _portugueseCultures = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures)
                .Where(c => c.TwoLetterISOLanguageName == "pt")
                .Select(c => c.Name.ToLowerInvariant())
                .ToList();

            _spanishCultures = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures)
                .Where(c => c.TwoLetterISOLanguageName == "es")
                .Select(c => c.Name.ToLowerInvariant())
                .ToList();
        }

        public async Task Invoke(HttpContext context)
        {
            // Lê o cabeçalho Accept-Language e pega apenas o primeiro idioma puro
            var requestedCulture = context.Request.Headers.AcceptLanguage
                .FirstOrDefault()?
                .Split(',').FirstOrDefault()?.Trim().ToLowerInvariant();

            var culture = new CultureInfo(_defaultCulture);

            if (requestedCulture.NotEmpty())
            {
                if (_portugueseCultures.Contains(requestedCulture) || requestedCulture.StartsWith("pt"))
                {
                    culture = new CultureInfo("pt-BR");
                }
                else if (_spanishCultures.Contains(requestedCulture) || requestedCulture.StartsWith("es"))
                {
                    culture = new CultureInfo("es");
                }
            }

            CultureInfo.CurrentCulture = culture; // Define a cultura atual para formatação de números, datas, moedas etc.
            CultureInfo.CurrentUICulture = culture; // Define a cultura usada para localizar recursos (como arquivos .resx) com base no idioma/região

            await _next(context); // chama esse próximo e mantém a execução fluindo corretamente.
        }
    }
}
