using LivroDeReceitas.Application.UseCases.Dashboard;
using LivroDeReceitas.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.API.Controllers
{
    public class DashboardController : LivroDeReceitasController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromServices] IDashboardUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Recipes.Any())
                return Ok(response);

            return NoContent();
        }
    }
}
