using LivroDeReceitas.Domain.Security.Cryptography;
using LivroDeReceitas.Infrastructure.Security.Cryptography;

namespace CommomTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        public static IPasswordEncripter Build() => new BCryptNet();
    }
}
