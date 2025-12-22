
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Data
{
    public abstract class UnitOfWorkBase
    {
        private readonly IDbContextBase _context;
        private IDbContextTransaction? _currentTransaction;
        private IScriptRepository? _scriptRepository;
        private IScriptConstraintRespository? _scriptConstraintRepository;

        public UnitOfWorkBase(IDbContextBase context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T? Get<T>() where T : class
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(IScriptRepository) || t == typeof(ScriptRepository):
                    return (_scriptRepository ?? new ScriptRepository(_context.Set<Script>())) as T;
                case Type t when t == typeof(IScriptConstraintRespository) || t == typeof(ScriptConstraintRespository):
                    return (_scriptConstraintRepository ?? new ScriptConstraintRespository(_context.Set<ScriptConstraint>())) as T;
                default:
                    throw new InvalidOperationException($"No repository found for type {typeof(T).Name}");
            }
        }

        public IScriptRepository GetScriptRepository() =>
                    Get<IScriptRepository>() ?? throw new InvalidOperationException("Script repository is not initialized.");


        public IScriptConstraintRespository GetScriptConstaintRepository() =>
                    Get<IScriptConstraintRespository>() ?? throw new InvalidOperationException("Script constraint repository is not initialized.");

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Rollback()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Rollback();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            _currentTransaction = _context.Database.BeginTransaction();
            return _currentTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }
    }

    public class CoreUnitOfWorkBase : UnitOfWorkBase, ICoreUnitOfWork
    {
        private readonly ISharedDbContext<CoreConnection> _context;
        public CoreUnitOfWorkBase(ISharedDbContext<CoreConnection> context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }

    public class CustomerUnitOfWorkBase : UnitOfWorkBase, ICustomerUnitOfWork
    {
        private readonly ISharedDbContext<DefaultConnection> _context;
        public CustomerUnitOfWorkBase(ISharedDbContext<DefaultConnection> context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
