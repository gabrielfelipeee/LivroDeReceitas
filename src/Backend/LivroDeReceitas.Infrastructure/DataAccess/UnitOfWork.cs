using LivroDeReceitas.Domain.Repositories;

namespace LivroDeReceitas.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LivroDeReceitasDbContext _dbContext;
        public UnitOfWork(LivroDeReceitasDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
