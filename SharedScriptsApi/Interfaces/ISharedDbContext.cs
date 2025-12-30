using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SharedScriptsApi.Data;

namespace SharedScriptsApi.Interfaces
{
    public interface IDbContextBase
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        IQueryable<TEntity> Entity<TEntity>(bool track = false) where TEntity : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        void Rollback();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task RollBackTransaction(CancellationToken cancellationToken);
        void Dispose();
    }

    public interface ISharedDbContext<TConnection> : IDbContextBase 
        where TConnection : IConnectionDetail 
    {}
}