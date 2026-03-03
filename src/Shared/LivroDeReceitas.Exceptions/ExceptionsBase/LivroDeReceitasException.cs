using System.Net;

namespace LivroDeReceitas.Exceptions.ExceptionsBase
{
    public abstract class LivroDeReceitasException : SystemException
    {
        protected LivroDeReceitasException(string message) : base(message) { }

        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
