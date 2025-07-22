using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistActiveUserWithEmail(string email);
        public Task<UserEntity?> GetByEmailAndPassword(string email, string password);
    }
}
