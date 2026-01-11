using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        public Task Add(Entities.User userEntity);
    }
}
