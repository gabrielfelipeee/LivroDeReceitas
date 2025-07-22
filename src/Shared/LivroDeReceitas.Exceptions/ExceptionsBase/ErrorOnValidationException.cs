namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : LivroDeReceitasException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errors) : base(string.Empty) => ErrorMessages = errors;

    }
}
