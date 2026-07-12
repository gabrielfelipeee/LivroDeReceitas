using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
    {
        private readonly LivroDeReceitasDbContext _dbContext;
        public UserRepository(LivroDeReceitasDbContext dbContext) => _dbContext = dbContext;


        public async Task Add(User userEntity) => await _dbContext.Users.AddAsync(userEntity);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Email.Equals(email) && user.Active);

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AsNoTracking().AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
        }

        public async Task<User?> GetByEmail(string email) => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));


        public async Task<User> GetById(long id) => await _dbContext.Users.FirstAsync(user => user.Id == id);

        public void Update(User userEntity) => _dbContext.Users.Update(userEntity);


        public async Task DeleteAccount(Guid userIdentifier)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserIdentifier == userIdentifier);
            if (user is null)
                return;

            IQueryable<Recipe> recipes = _dbContext.Recipes.Where(recipe => recipe.UserId == user.Id);
            _dbContext.Recipes.RemoveRange(recipes);

            _dbContext.Users.Remove(user);
        }
    }
}
