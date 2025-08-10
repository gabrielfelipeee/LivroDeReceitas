using LivroDeReceitas.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt;      
using System.Security.Claims;         
using System.Text;                            

namespace LivroDeReceitas.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes; // Tempo de expiração do token (em minutos)
        private readonly string _signinKey;           // Chave secreta usada para assinar o token

        public JwtTokenGenerator(uint expirationTimeMinutes, string signinKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signinKey = signinKey;
        }

        // Método para gerar o token JWT
        public string Generate(Guid userIdentifier)
        {
            // Lista de claims (informações) que estarão dentro do token
            var claims = new List<Claim>()
            {
                // Claim com o identificador do usuário (Sid = Security Identifier)
                new Claim(ClaimTypes.Sid, userIdentifier.ToString())
            };

            // Descrição do token a ser criado
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Define quando o token vai expirar
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),

                // Define a credencial de assinatura com a chave secreta e o algoritmo HMAC-SHA256
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),

                // Define quem é o "dono" do token (as claims)
                Subject = new ClaimsIdentity(claims)
            };

            // Criador e manipulador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Cria o token baseado na descrição
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // Converte o token para string no formato JWT
            return tokenHandler.WriteToken(securityToken);
        }


        // Método auxiliar para gerar a chave simétrica usada na assinatura
        private SymmetricSecurityKey SecurityKey()
        {
            // Converte a chave secreta em um array de bytes (UTF-8)
            var bytes = Encoding.UTF8.GetBytes(_signinKey);

            // Cria e retorna a chave simétrica
            return new SymmetricSecurityKey(bytes);
        }
    }
}
