using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;
        public Repository(DataContext dbContext)
        {
            _context = dbContext;
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }   

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        async Task<IEnumerable<IScript>> GetScripts(string name, string version, bool includeConstraints, DateTime? modifiedDate)
        {
            IQueryable<Script> query = this._context.Set<Script>()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Active);

            if (!string.IsNullOrWhiteSpace(version))
            {
                query = query.Where(x => x.Version == version);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name == name);
            }

            if (includeConstraints)
            {
                query = query.Include(x => x.ScriptConstraints != null
                    ? x.ScriptConstraints.Where(c => c.Active);
            }

            if (modifiedDate != null)
            {
                query = query
                    .Where(x => x.ModifiedDate > modifiedDate || x.ScriptConstraints != null && x.ScriptConstraints.Any(y => y.ModifiedDate > modifiedDate));
            }

            return await query.ToListAsync();
        }
    }
}
