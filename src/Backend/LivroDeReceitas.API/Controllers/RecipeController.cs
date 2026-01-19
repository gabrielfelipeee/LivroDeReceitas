using LivroDeReceitas.API.Attributes;
using LivroDeReceitas.Application.UseCases.Recipe.Filter;
using LivroDeReceitas.Application.UseCases.Recipe.Register;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
    [AuthenticatedUser]
    public class RecipeController : LivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RequestRecipeJson request, [FromServices] IRegisterRecipeUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Filter([FromBody] RequestFilterRecipeJson request, [FromServices] IFilterRecipeUseCase useCase)
        {
            var response = await useCase.Execute(request);

            if (response.Recipes.Any())
                return Ok(response);

            return NoContent();
        }
    }
}
