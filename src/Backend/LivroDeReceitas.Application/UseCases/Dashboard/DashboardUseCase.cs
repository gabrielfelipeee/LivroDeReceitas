using AutoMapper;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;

namespace LivroDeReceitas.Application.UseCases.Dashboard
{
    public class DashboardUseCase : IDashboardUseCase
    {
        private readonly ILoggedUser _loggedUseer;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IMapper _mapper;
        public DashboardUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeReadOnlyRepository,
            IMapper mapper
            )
        {
            _loggedUseer = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var loggedUser = await _loggedUseer.User();

            var recipes = await _recipeReadOnlyRepository.GetForDashboard(loggedUser);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
            };
        }
    }
}
