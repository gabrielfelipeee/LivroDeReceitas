using LivroDeReceitas.API.Attributes;
using LivroDeReceitas.API.Binders;
using LivroDeReceitas.Application.UseCases.Recipe.Delete;
using LivroDeReceitas.Application.UseCases.Recipe.Filter;
using LivroDeReceitas.Application.UseCases.Recipe.Generate;
using LivroDeReceitas.Application.UseCases.Recipe.GetById;
using LivroDeReceitas.Application.UseCases.Recipe.Register;
using LivroDeReceitas.Application.UseCases.Recipe.Update;
using LivroDeReceitas.Application.UseCases.Recipe.Image;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
    [AuthenticatedUser]
    public class RecipeController : LivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RequestRecipeJson request, [FromServices] IRegisterRecipeUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateRecipeUseCase useCase,
            [FromBody] RequestRecipeJson request,
            [FromRoute][ModelBinder(typeof(LivroDeReceitasIdBinder))] long id
            )
        {
            await useCase.Execute(id, request);

            return NoContent();
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

        [HttpPost("generate")]
        [ProducesResponseType(typeof(ResponseGenerateRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Generate([FromBody] RequestGenerateRecipeJson request, [FromServices] IGenerateRecipeUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }

        [HttpPut("image/{id}")]
        [ProducesResponseType(typeof(ResponseGenerateRecipeJson), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateImage(
            [FromRoute][ModelBinder(typeof(LivroDeReceitasIdBinder))] long id,
            IFormFile file,
            [FromServices] IAddUpdateImageCoverUseCase useCase)
        {
            await useCase.Execute(id, file);

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
