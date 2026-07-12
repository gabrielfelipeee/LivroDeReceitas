using LivroDeReceitas.Application.UseCases.Login.DoLogin;
using LivroDeReceitas.Application.UseCases.Login.External;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LivroDeReceitas.API.Controllers
{
    public class LoginController : LivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestLoginJson request)
        {
            var result = await useCase.Execute(request);
            return Ok(result);
        }

        [HttpGet("google")]
        public async Task<IActionResult> LoginWithGoogle([FromServices] IExternalLoginUseCase useCase, [FromQuery] string returnUrl)
        {
            var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (IsNotAuthenticated(authenticate))
                return Challenge(GoogleDefaults.AuthenticationScheme);
            else
            {
                var claims = authenticate.Principal!.Identities.First().Claims;

                var name = claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                var email = claims.First(claim => claim.Type == ClaimTypes.Email).Value;

                var token = await useCase.Execute(name, email);

                return Redirect($"{returnUrl}/{token}");
            }
        }
    }
}
