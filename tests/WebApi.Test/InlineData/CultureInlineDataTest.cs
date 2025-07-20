using System.Collections;

namespace WebApi.Test.InlineData
{
    public class CultureInlineDataTest : IEnumerable<object[]>
    {

        // Retorna um array de object
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "pt-BR" };
            yield return new object[] { "es" };
            yield return new object[] { "en" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
