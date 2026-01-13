using Sqids;

namespace CommomTestUtilities.IdEncryption
{
    public class IdEncripterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new SqidsOptions()
            {
                MinLength = 10,
                Alphabet = "3itchfSwnkeAW2H6zYTsraIVUjv1MoxEB4X9bmKPRDZJyg8GpFqL75ldNuQOC"
            });
        }
    }
}
