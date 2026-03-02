using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeReadOnlyRepository,
            IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long recipeId)
        {
            var user = await _loggedUser.User();

            if (!await _recipeReadOnlyRepository.ExistActiveRecipeWithId(user, recipeId))
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            await _recipeWriteOnlyRepository.Delete(recipeId);
            await _unitOfWork.Commit();
        }
    }
}
