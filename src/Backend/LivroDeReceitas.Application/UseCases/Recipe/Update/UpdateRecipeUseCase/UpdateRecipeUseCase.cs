using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Update.UpdateRecipeUseCase
{
    public class UpdateRecipeUseCase : IUpdateRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeUpdateOnlyRepository _recipeUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateRecipeUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _recipeUpdateOnlyRepository = recipeUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Execute(long recipeId, RequestRecipeJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var recipe = await _recipeUpdateOnlyRepository.GetById(loggedUser, recipeId)
                ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            recipe.Instructions.Clear();
            recipe.Ingredients.Clear();
            recipe.DishTypes.Clear();

            _mapper.Map(request, recipe);

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (var index = 0; index < instructions.Count; index++)
                instructions.ElementAt(index).Step = index + 1;

            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            _recipeUpdateOnlyRepository.Update(recipe);
            await _unitOfWork.Commit();
        }

        private static void Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if (result.IsValid.IsFalse())
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}
