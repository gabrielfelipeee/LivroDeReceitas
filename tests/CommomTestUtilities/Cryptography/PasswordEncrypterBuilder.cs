using LivroDeReceitas.Application.Services.Cryptography;

namespace CommomTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        public static PasswordEncrypter Build() => new PasswordEncrypter("abc123");
    }
}
