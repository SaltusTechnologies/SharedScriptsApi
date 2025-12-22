
using Microsoft.EntityFrameworkCore.Storage;

namespace SharedScriptsApi.Interfaces
{
    public interface IUnitOfWork
    {
        T? Get<T>() where T : class;
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        void Rollback();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }

    public interface ICoreUnitOfWork : IUnitOfWork
    {
        // Add Core-specific members here if needed in the future
    }

    public interface ICustomerUnitOfWork : IUnitOfWork
    {
        // Add Customer-specific members here if needed in the future
    }
}


