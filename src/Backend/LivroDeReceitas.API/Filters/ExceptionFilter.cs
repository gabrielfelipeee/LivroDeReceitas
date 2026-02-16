using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LivroDeReceitas.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is LivroDeReceitasException livroDeReceitasException)
                HandleProjectException(context, livroDeReceitasException);
            else
                ThrowUnknowException(context);
        }

        private static void HandleProjectException(ExceptionContext context, LivroDeReceitasException livroDeReceitasException)
        {
            context.HttpContext.Response.StatusCode = (int)livroDeReceitasException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(livroDeReceitasException.GetErrorMessages()));
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
