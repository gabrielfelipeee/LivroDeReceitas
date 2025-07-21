using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.DataAccess.Repository
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
    {
        private readonly LivroDeReceitasDbContext _dbContext;
        public UserRepository(LivroDeReceitasDbContext dbContext) => _dbContext = dbContext;


        public async Task Add(UserEntity userEntity) => await _dbContext.Users.AddAsync(userEntity);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
    }
}
