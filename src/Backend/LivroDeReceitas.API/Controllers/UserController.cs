using LivroDeReceitas.API.Attributes;
using LivroDeReceitas.Application.UseCases.User.ChangePassword;
using LivroDeReceitas.Application.UseCases.User.Delete.Request;
using LivroDeReceitas.Application.UseCases.User.Profile;
using LivroDeReceitas.Application.UseCases.User.Register;
using LivroDeReceitas.Application.UseCases.User.Update;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUser]
        public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> Update([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUserJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase, [FromBody] RequestChangePasswordJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthenticatedUser]
        public async Task<IActionResult> Delete([FromServices] IRequestDeleteUserUseCase useCase)
        {
            await useCase.Execute();
            return NoContent();
        }
    }
}
