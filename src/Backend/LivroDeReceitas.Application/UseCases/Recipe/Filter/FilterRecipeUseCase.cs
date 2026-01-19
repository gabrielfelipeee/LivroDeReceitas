using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        public FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
        }

        public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var filters = new Domain.Dtos.FilterRecipeDto
            {
                RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
                CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
                Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
                DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList(),
            };

            var result = await _recipeReadOnlyRepository.Filter(loggedUser, filters);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(result)
            };
        }

        private static void Validate(RequestFilterRecipeJson request)
        {
            var validator = new FilterRecipeValidator();

            var result = validator.Validate(request);

            if (result.IsValid.IsFalse())
            {
                var errorsMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
