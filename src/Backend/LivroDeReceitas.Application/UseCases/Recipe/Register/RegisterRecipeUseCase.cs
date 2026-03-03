using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories;
using LivroDeReceitas.Domain.Repositories.Recipe;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterRecipeUseCase(
            IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser
            )
        {
            _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.Id;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (var index = 0; index < instructions.Count; index++)
                instructions[index].Step = index + 1;
            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            await _recipeWriteOnlyRepository.Add(recipe);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
        }

        private static void Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if (result.IsValid.IsFalse())
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}
