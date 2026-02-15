namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public class NotFoundException : LivroDeReceitasException
    {
        public NotFoundException(string message) : base(message)
        {}
    }
}
