using System.Net;

namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : LivroDeReceitasException
    {
        private readonly IList<string> _errorMessages;
        public ErrorOnValidationException(IList<string> errors) : base(string.Empty) => _errorMessages = errors;

        public override IList<string> GetErrorMessages() => _errorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
