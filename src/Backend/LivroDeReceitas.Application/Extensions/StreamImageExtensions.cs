using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace LivroDeReceitas.Application.Extensions
{
    public static class StreamImageExtensions
    {
        public static (bool isValidImage, string extension) ValidateAndGetImageExtension(this Stream stream)
        {
            var result = (false, string.Empty);

            if (stream.Is<PortableNetworkGraphic>())
                result = (true, NormalizeExtension(PortableNetworkGraphic.TypeExtension));
            else if (stream.Is<JointPhotographicExpertsGroup>())
                result = (true, NormalizeExtension(JointPhotographicExpertsGroup.TypeExtension));

            // Reseta a posição do stream para o início, pois a validação do tipo do arquivo já 'consumiu' parte dos bytes.
            stream.Position = 0;

            return result;
        }

        private static string NormalizeExtension(string extension) => extension.StartsWith('.') ? extension : $".{extension}";

    }
}
