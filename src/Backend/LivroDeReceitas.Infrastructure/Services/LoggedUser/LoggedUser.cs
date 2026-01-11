using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Domain.Services.LoggedUser;
using LivroDeReceitas.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LivroDeReceitas.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly LivroDeReceitasDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;
        public LoggedUser(LivroDeReceitasDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var userIdentifier = Guid.Parse(identifier);

            return await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);
        }
    }
}
