using System.Net;

namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class NotFoundException : LivroDeReceitasException
    {
        public NotFoundException(string message) : base(message)
        {}

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
