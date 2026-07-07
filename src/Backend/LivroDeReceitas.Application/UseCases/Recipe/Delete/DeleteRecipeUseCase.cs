using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeReadOnlyRepository,
            IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
            IBlobStorageService blobStorageService,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long recipeId)
        {
            var user = await _loggedUser.User();

            var recipe = await _recipeReadOnlyRepository.GetById(user, recipeId)
                ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            if (!string.IsNullOrWhiteSpace(recipe.ImageIdentifier))
                await _blobStorageService.Delete(user, recipe.ImageIdentifier);

            await _recipeWriteOnlyRepository.Delete(recipeId);
            await _unitOfWork.Commit();
        }
    }
}
