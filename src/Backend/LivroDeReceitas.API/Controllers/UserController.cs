using LivroDeReceitas.API.Attributes;
using LivroDeReceitas.Application.UseCases.User.Register;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
    [AuthenticatedUser()]
    public class UserController : LivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase, // Outra forma de pegar a injeção de dependência
            [FromBody] RequestRegisterUserJson request) // Body da requisição
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }
    }
}
