using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        public Task<Entities.User> GetById(long id);
        public void Update(Entities.User userEntity);
    }
}
