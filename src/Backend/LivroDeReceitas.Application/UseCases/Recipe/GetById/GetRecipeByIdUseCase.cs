using AutoMapper;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _recipeReadOnlyRepository.GetById(loggedUser, recipeId);

            return recipe is null
                ? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND)
                : _mapper.Map<ResponseRecipeJson>(recipe);
        }
    }
}
