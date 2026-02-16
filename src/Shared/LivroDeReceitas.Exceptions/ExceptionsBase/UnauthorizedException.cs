using System.Net;

namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class UnauthorizedException : LivroDeReceitasException
    {
        public UnauthorizedException(string message) : base(message)
        { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
