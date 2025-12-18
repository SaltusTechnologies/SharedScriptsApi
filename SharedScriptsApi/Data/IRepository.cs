using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.Interfaces.Saltus.digiTICKET.ExternalSources.Models;
using System.Linq.Expressions;
using System.Security.Principal;

namespace SharedScriptsApi.Data
{
    public interface IRepository<TEntity>
    where TEntity : class
    {
        Dictionary<string, Func<TEntity, bool>>? Filters { get; }
        DbSet<TEntity> Entities { get; }
        // Asynchronous methods
        Task<TEntity?> GetAsync(int id, bool track = false, CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync(object[] primaryKey, bool track = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>?> GetAllAsync(bool track = false, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        // Synchronous methods
        void Update(TEntity entity);
        void Add(TEntity entity);
        TEntity? Get(int id, bool track = false);
        TEntity? Get(object[] primaryKey, bool track = false);
        IEnumerable<TEntity>? GetAll(bool track = false);
        IQueryable<TEntity>? Query(Expression<Func<TEntity, bool>> exp, bool track = false);
        Task<IEnumerable<TEntity>?> QueryMultipleAsync(Expression<Func<TEntity, bool>> exp, bool track = false);
        IEnumerable<TEntity>? QueryMultiple(Expression<Func<TEntity, bool>> exp, bool track = false);
        TEntity? QuerySingle(Expression<Func<TEntity, bool>> exp, bool track = false);
        Task<TEntity?> QuerySingleAsync(Expression<Func<TEntity, bool>> exp, bool track = false);
        IQueryable<TEntity>? Query(bool track = false);
    }
}