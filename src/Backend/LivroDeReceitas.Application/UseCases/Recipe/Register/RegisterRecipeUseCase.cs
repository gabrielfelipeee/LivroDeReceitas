using AutoMapper;
using LivroDeReceitas.Application.Extensions;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterRecipeUseCase(
            IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
            IBlobStorageService blobStorageService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser
            )
        {
            _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.Id;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (var index = 0; index < instructions.Count; index++)
                instructions[index].Step = index + 1;
            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            if (request.Image is not null)
            {
                var fileStream = request.Image.OpenReadStream();

                (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();
                if (isValidImage.IsFalse())
                    throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

                recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

                await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
            }

            await _recipeWriteOnlyRepository.Add(recipe);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
        }

        private static void Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if (result.IsValid.IsFalse())
                throw new ErrorOnValidationException([.. result.Errors.Select(e => e.ErrorMessage).Distinct()]);
        }
    }
}
