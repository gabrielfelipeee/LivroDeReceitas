using System.Security.Cryptography;
using System.Text;

namespace LivroDeReceitas.Application.Services.Cryptography
{
    public class PasswordEncrypter
    {
        private readonly string _additionalKey;
        public PasswordEncrypter(string additionalKey) => _additionalKey = additionalKey;


        // Método público que recebe uma senha em texto  e retorna seu hash como string hexadecimal
        public string Encrypt(string password)
        {
            var newPassword = $"{password}{_additionalKey}";

            // Converte a string da senha para um array de bytes usando codificação UTF-8
            var bytes = Encoding.UTF8.GetBytes(newPassword);

            // Aplica o algoritmo SHA-512 para gerar o hash a partir dos bytes da senha
            var hashBytes = SHA512.HashData(bytes);

            // Converte os bytes do hash em uma string hexadecimal e retorna
            return StringBytes(hashBytes);
        }

        // Converte um hash de bytes em uma string hexadecimal
        private static string StringBytes(byte[] bytes)
        {
            // StringBuilder é mais eficiente para montar strings em loops
            var sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                // Converte o byte para uma string hexadecimal com dois dígitos (ex: 255 => "ff")
                var hex = b.ToString("x2");

                // Adiciona o valor hexadecimal ao StringBuilder
                sb.Append(hex);
            }

            // Retorna a string final com todos os valores hexadecimais concatenados
            return sb.ToString();
        }
    }
}
