using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LivroDeReceitas.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandler
    {
        // Método auxiliar para gerar a chave simétrica usada na assinatura
        protected static SymmetricSecurityKey SecurityKey(string signinKey)
        {
            // Converte a chave secreta em um array de bytes (UTF-8)
            var bytes = Encoding.UTF8.GetBytes(signinKey);

            // Cria e retorna a chave simétrica
            return new SymmetricSecurityKey(bytes);
        }
    }
}
