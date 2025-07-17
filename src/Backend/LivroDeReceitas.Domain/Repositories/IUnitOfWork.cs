namespace LivroDeReceitas.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
