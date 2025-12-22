using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.DataModels;

namespace SharedScriptsApi.Data
{
    public class ScriptConstraintRespository : Repository<ScriptConstraint>, IScriptConstraintRespository
    {
        #region fields/properties

        protected DbSet<ScriptConstraint> ScriptConstraints => base.Entities;

        DbSet<ScriptConstraint> IScriptConstraintRespository.ScriptConstraints => ScriptConstraints;
        #endregion

        #region .ctors
        public ScriptConstraintRespository(DbSet<ScriptConstraint> entities) : base(entities) { }
        #endregion

        async Task<ScriptConstraint?> IScriptConstraintRespository.AddScriptConstraint(ScriptConstraint scriptConstraint)
        {
            var result = await Entities.AddAsync(scriptConstraint);
            return result.Entity;
        }

        async Task<IEnumerable<ScriptConstraint>?> IScriptConstraintRespository.GetScriptConstraints(string? name, string? version, DateTime? modifiedDate)
        {
            IQueryable<ScriptConstraint> query = Entities
                .AsNoTrackingWithIdentityResolution();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(version))
            {
                query = query.Where(x => x.Version == version);
            }

            if (modifiedDate != null)
            {
                query = query.Where(x => x.ModifiedDate > modifiedDate);
            }

            return await query.ToListAsync();
        }

        public async Task<ScriptConstraint?> GetScriptConstraintById(int scriptConstraintId, bool track)
        {
            IQueryable<ScriptConstraint> query = Entities;
            if (!track)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return await query
                .FirstOrDefaultAsync(x => x.ScriptConstraintId == scriptConstraintId);
        }

        async Task<ScriptConstraint?> IScriptConstraintRespository.UpdateScriptConstraint(ScriptConstraint scriptConstraint)
        {
            ScriptConstraint? scriptConstraintToUpdate = await this.GetScriptConstraintById(scriptConstraint.ScriptConstraintId, track: true);
            Entities.Entry(scriptConstraintToUpdate!).CurrentValues.SetValues(scriptConstraint);
            return await this.GetScriptConstraintById(scriptConstraintToUpdate!.ScriptConstraintId, false);
        }
    }

    public interface IScriptConstraintRespository : IRepository<ScriptConstraint>
    {
        DbSet<ScriptConstraint> ScriptConstraints { get; }
        Task<ScriptConstraint?> AddScriptConstraint(ScriptConstraint scriptConstraint);
        Task<IEnumerable<ScriptConstraint>?> GetScriptConstraints(string? name = null, string? version = null, DateTime? modifiedDate = null);
        Task<ScriptConstraint?> GetScriptConstraintById(int scriptConstraintId, bool track = false);
        Task<ScriptConstraint?> UpdateScriptConstraint(ScriptConstraint scriptConstraint);
    }
}
