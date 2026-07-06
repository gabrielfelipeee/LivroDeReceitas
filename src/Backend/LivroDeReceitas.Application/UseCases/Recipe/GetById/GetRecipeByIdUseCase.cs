using AutoMapper;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IBlobStorageService _blobStorageService;
        public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _recipeReadOnlyRepository.GetById(loggedUser, recipeId)
                ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            var response = _mapper.Map<ResponseRecipeJson>(recipe);

            if (!string.IsNullOrWhiteSpace(recipe.ImageIdentifier))
            {
                var url = await _blobStorageService.GetImageUrl(loggedUser, recipe.ImageIdentifier);

                response.ImageUrl = url;
            }

            return response;
        }
    }
}
