using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositories.User;
using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace LivroDeReceitas.API.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {

        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _accessTokenValidator = accessTokenValidator;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);

                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                var existUser = await _userReadOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);
                if (existUser.IsFalse())
                    throw new LivroDeReceitasException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
            // Caso o token está expirado
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
                {
                    TokenIsExpired = true
                });
            }
            // Caso a exceção for do tipo LivroDeReceitasException
            catch (LivroDeReceitasException ex)
            {
                // Já mostra a mensagem direto, pois aqui só vai cair exception do tipo LivroDeReceitasException 
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
            }
            // Caso não for umas das exceptions acima
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
            }
        }

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            if (authentication.NotEmpty().IsFalse())
                throw new LivroDeReceitasException(ResourceMessagesException.NO_TOKEN);


            // O token irá vir assim: "Bearer asdf1234xyz..."
            // Então para pegar somente o token é necessário tirar o palavra Bearer e o espaço
            // ["Bearer ".Length..] -> Pegue a substring a partir do índice 7 até o final da string: "asdf1234xyz..."
            return authentication["Bearer ".Length..].Trim();
        }
    }
}
