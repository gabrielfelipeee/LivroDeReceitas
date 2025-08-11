using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Infrastructure.Security.Tokens.Access.Generator;
namespace CommomTestUtilities.Tokens
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signinKey: "z6H5w2P9dM8VnQfLxJ1TgKz7YsEw3CpL");
    }
}
