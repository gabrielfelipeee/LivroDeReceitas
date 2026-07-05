using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;

namespace LivroDeReceitas.Application.UseCases.Recipe.Image
{
    public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IRecipeUpdateOnlyRepository _recipeUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddUpdateImageCoverUseCase(
            ILoggedUser loggedUser,
            IBlobStorageService blobStorageService,
            IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _blobStorageService = blobStorageService;
            _recipeUpdateOnlyRepository = recipeUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long recipeId, IFormFile file)
        {
            var stream = Validate(file);

            var loggedUser = await _loggedUser.User();

            var recipe = await _recipeUpdateOnlyRepository.GetById(loggedUser, recipeId)
                ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            if (string.IsNullOrWhiteSpace(recipe.ImageIdentifier))
            {
                recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                _recipeUpdateOnlyRepository.Update(recipe);
                await _unitOfWork.Commit();
            }

            // Reseta a posição do stream para o início, pois a validação do tipo do arquivo já 'consumiu' parte dos bytes.
            stream.Position = 0;

            await _blobStorageService.Upload(loggedUser, stream, recipe.ImageIdentifier);
        }

        private static Stream Validate(IFormFile file)
        {
            var fileStream = file.OpenReadStream();

            // PNG || JPG/JPEG
            if ((fileStream.Is<PortableNetworkGraphic>() || fileStream.Is<JointPhotographicExpertsGroup>()).IsFalse())
                throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

            return fileStream;
        }
    }
}
