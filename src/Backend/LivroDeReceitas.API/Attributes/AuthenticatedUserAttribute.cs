using LivroDeReceitas.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Attributes
{
    // Define um atributo customizado
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] // Este atributo só pode ser aplicado em: classes e Métodos
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        // "base(typeof(AuthenticatedUserFilter))" indica que este atributo usará o filtro AuthenticatedUserFilter
        // TypeFilterAttribute é um recurso do ASP.NET Core que cria uma ponte entre um atributo decorativo ([AuthenticatedUser]) e um filtro real (AuthenticatedUserFilter).
        // Quando o framework encontra [AuthenticatedUser] em uma action ou controller, ele resolve uma instância de AuthenticatedUserFilter **usando o container de DI**.
        // Isso permite que o filtro receba dependências via construtor (IAccessTokenValidator, IUserReadOnlyRepository) sem precisar instanciá-las manualmente.
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
    }
}
