using LivroDeReceitas.Domain.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LivroDeReceitasController : ControllerBase
    {
        protected static bool IsNotAuthenticated(AuthenticateResult authenticateResult)
        {
            return authenticateResult.Succeeded.IsFalse()
                || authenticateResult.Principal is null
                || authenticateResult.Principal.Identities.Any(x => x.IsAuthenticated).IsFalse();
        }
    }
}
