using LivroDeReceitas.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LivroDeReceitas.Infrastructure.Security.Tokens.Access.Validator
{
    // Classe responsável por validar o token JWT recebido
    public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
    {
        private readonly string _signinKey;
        public JwtTokenValidator(string signinKey) => _signinKey = signinKey;


        // Método que valida o token e retorna o identificador do usuário (Guid)
        public Guid ValidateAndGetUserIdentifier(string token)
        {
            // Define os parâmetros de validação do token
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // Não validar "quem vai usar" (audience)
                ValidateIssuer = false, // Não validar "quem emitiu" (issuer)
                ValidateIssuerSigningKey = true, // Ativa validação de assinatura
                IssuerSigningKey = SecurityKey(_signinKey), // Chave secreta para validar a assinatura
                ValidateLifetime = true, // Ativa validação de Tempo de expiração
                ClockSkew = new TimeSpan(0) // Sem tolerância de tempo (token expira exatamente no horário definido)
            };

            // Manipulador para ler e validar tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Valida o token de acordo com os parâmetros acima
            // "principal" representa o usuário autenticado (com claims)
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            // Pega a claim que contém o identificador do usuário (ClaimTypes.Sid)
            var userIdentifier = principal.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            // Converte o valor para Guid e retorna
            return Guid.Parse(userIdentifier);
        }
    }
}
