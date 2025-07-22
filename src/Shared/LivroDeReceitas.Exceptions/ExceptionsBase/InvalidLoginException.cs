namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : LivroDeReceitasException
    {
        public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
        { }
    }
}
