using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        public Task<UserEntity> GetById(long id);
        public void Update(UserEntity userEntity);
    }
}
