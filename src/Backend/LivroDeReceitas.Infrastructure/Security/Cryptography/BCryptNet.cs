using LivroDeReceitas.Domain.Security.Cryptography;

namespace LivroDeReceitas.Infrastructure.Security.Cryptography
{
    public class BCryptNet : IPasswordEncripter
    {
        public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool IsValid(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(text: password, hash: passwordHash);
    }
}
