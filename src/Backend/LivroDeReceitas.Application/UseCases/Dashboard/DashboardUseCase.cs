using AutoMapper;
using LivroDeReceitas.Application.Extensions;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.Storage;

namespace LivroDeReceitas.Application.UseCases.Dashboard
{
    public class DashboardUseCase : IDashboardUseCase
    {
        private readonly ILoggedUser _loggedUseer;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        public DashboardUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeReadOnlyRepository,
            IBlobStorageService blobStorageService,
        IMapper mapper
            )
        {
            _loggedUseer = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var loggedUser = await _loggedUseer.User();

            var recipes = await _recipeReadOnlyRepository.GetForDashboard(loggedUser);

            return new ResponseRecipesJson
            {
                Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
            };
        }
    }
}
