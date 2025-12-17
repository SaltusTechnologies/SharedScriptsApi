
using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.Interfaces.Saltus.digiTICKET.ExternalSources.Models;
using System.Linq.Expressions;

namespace SharedScriptsApi.Data
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        protected DbSet<T> Entities { get; set; }

        DbSet<T> IRepository<T>.Entities => Entities!;

        protected virtual Dictionary<string, Func<T, bool>>? Filters => new Dictionary<string, Func<T, bool>>();

        Dictionary<string, Func<T, bool>>? IRepository<T>.Filters => Filters;

        public Repository(DbSet<T> entities)
        {
            Entities = entities ?? throw new ArgumentNullException(nameof(entities), "Entities cannot be null.");
        }

        // Synchronous methods
        public virtual void Add(T entity)
        {
            Entities.Add(entity);
        }

        public virtual T? Get(int id, bool track = false)
        {
            return Query(track).FirstOrDefault(x => x.GetId() == id && x.Active);
        }

        T? IRepository<T>.Get(object[] primaryKey, bool track)
        {
            return Query(track).FirstOrDefault(x => x.GetPrimaryKey().SequenceEqual(primaryKey) && x.Active);
        }

        public virtual IEnumerable<T>? GetAll(bool track = false)
        {
            return track
                ? Query(track).ToList()
                : [.. Query(track).Where(x => x.Active)];
        }

        public virtual void Update(T entity)
        {
            Entities.Update(entity);
        }

        public virtual IQueryable<T> Query(bool track = false)
        {
            return track ? Entities : Entities.AsNoTrackingWithIdentityResolution();
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> exp, bool track = false)
        {
            return track ? this.Entities.Where(exp) : Entities.AsNoTrackingWithIdentityResolution().Where(exp);
        }

        public async Task<IEnumerable<T>?> QueryMultipleAsync(Expression<Func<T, bool>> exp, bool track = false)
        {
            return await Query(exp, track).ToListAsync();
        }

        public IEnumerable<T>? QueryMultiple(Expression<Func<T, bool>> exp, bool track = false)
        {
            return [.. Query(exp, track)];
        }

        public T? QuerySingle(Expression<Func<T, bool>> exp, bool track = false)
        {
            return Query(exp, track).FirstOrDefault();
        }

        public async Task<T?> QuerySingleAsync(Expression<Func<T, bool>> exp, bool track = false)
        {
            return await Query(exp, track).FirstOrDefaultAsync();
        }

        // Async methods
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Entities.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<T?> GetAsync(int id, bool track, CancellationToken cancellationToken = default)
        {
            return await Query(track).FirstOrDefaultAsync(x => x.GetId() == id, cancellationToken);
        }

        async Task<T?> IRepository<T>.GetAsync(object[] primaryKey, bool track, CancellationToken cancellationToken)
        {
            return await Query(track)
                .FirstOrDefaultAsync(x => x.GetPrimaryKey().SequenceEqual(primaryKey) && x.Active, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>?> GetAllAsync(bool track = false, CancellationToken cancellationToken = default)
        {
            return await Query(track).Where(x => x.Active)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Entities.Update(entity);
            await Task.CompletedTask;
        }
    }
}
