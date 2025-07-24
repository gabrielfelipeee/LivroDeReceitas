using System.Net.Http.Json;

namespace WebApi.Test
{
    public class LivroDeReceitasClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        // WebApplicationFactory configura o ambiente de teste com suporte a DI e pipeline completo da aplicação
        private readonly HttpClient _httpClient;      
        public LivroDeReceitasClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(method, request);
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }
    }
}
