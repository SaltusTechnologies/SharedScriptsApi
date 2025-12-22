
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Data
{
    public abstract class DbContextBase : DbContext, IDbContextBase
    {
        async Task<int> IDbContextBase.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await SaveChangesAsync(cancellationToken);
        }

        int IDbContextBase.SaveChanges()
        {
            return SaveChanges();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task RollBackTransaction(CancellationToken cancellationToken)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
        }

        IDbContextTransaction IDbContextBase.BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        void IDbContextBase.Rollback()
        {
            Database.RollbackTransaction();
        }

        public IQueryable<TEntity> Entity<TEntity>(bool tracking) where TEntity : class
        {
            return tracking ? Set<TEntity>() : Set<TEntity>().AsNoTracking();
        }

        public abstract DbSet<TEntity> Entity<TEntity>() where TEntity : class;
    }
}
