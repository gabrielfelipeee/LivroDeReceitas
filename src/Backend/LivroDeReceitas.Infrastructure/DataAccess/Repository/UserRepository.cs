using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.DataAccess.Repository
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
    {
        private readonly LivroDeReceitasDbContext _dbContext;
        public UserRepository(LivroDeReceitasDbContext dbContext) => _dbContext = dbContext;


        public async Task Add(UserEntity userEntity) => await _dbContext.Users.AddAsync(userEntity);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Email.Equals(email) && user.Active);

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AsNoTracking().AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

        public async Task<UserEntity?> GetByEmailAndPassword(string email, string password)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
        }

        public async Task<UserEntity> GetById(long id) => await _dbContext.Users.FirstAsync(user => user.Id == id);

        public void Update(UserEntity userEntity) => _dbContext.Users.Update(userEntity);
    }
}
