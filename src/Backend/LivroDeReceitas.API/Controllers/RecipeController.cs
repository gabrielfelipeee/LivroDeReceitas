using LivroDeReceitas.API.Attributes;
using LivroDeReceitas.API.Binders;
using LivroDeReceitas.Application.UseCases.Recipe.Delete;
using LivroDeReceitas.Application.UseCases.Recipe.Filter;
using LivroDeReceitas.Application.UseCases.Recipe.GetById;
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute][ModelBinder(typeof(LivroDeReceitasIdBinder))] long id, [FromServices] IGetRecipeByIdUseCase useCase)
        {
            var response = await useCase.Execute(id);

            if (response is not null)
                return Ok(response);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute][ModelBinder(typeof(LivroDeReceitasIdBinder))] long id, [FromServices] IDeleteRecipeUseCase useCase)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
