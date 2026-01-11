using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
