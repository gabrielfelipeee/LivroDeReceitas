using LivroDeReceitas.Domain.Security.Tokens;

namespace LivroDeReceitas.API.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpContextTokenValue(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        public string Value()
        {
            // token
            var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();


            // O token irá vir assim: "Bearer asdf1234xyz..."
            // Então para pegar somente o token é necessário tirar o palavra Bearer e o espaço
            // ["Bearer ".Length..] -> Pegue a substring a partir do índice 7 até o final da string: "asdf1234xyz..."
            return authentication["Bearer ".Length..].Trim();
        }
    }
}
