using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Security.Cryptography;
using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public DoLoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetByEmail(request.Email);

            if (user is null || _passwordEncripter.IsValid(password: request.Password, passwordHash: user.Password).IsFalse())
                throw new InvalidLoginException();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }
    }
}
