

using Microsoft.EntityFrameworkCore;
using Saltus.digiTICKET.Data0111000000.Models;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Interfaces;
using System;

namespace SharedScriptsApi.Data
{
    public class ScriptRepository : Repository<Script>, IRepository<Script>, IScriptRepository
    {
        #region fields/properties

        protected DbSet<Script> Scripts => Entities;

        DbSet<Script> IScriptRepository.Scripts => Scripts;

        #endregion

        #region .ctors
        public ScriptRepository(DbSet<Script> dbSet) : base(dbSet)
        {
        }
        #endregion

        async Task<IEnumerable<Script>?> IScriptRepository.GetScripts(string branch, string name, string version, bool includeConstraints, DateTime? modifiedDate)
        {

            IQueryable<Script> query = Scripts.Where(x => x.Active);

            if (!string.IsNullOrWhiteSpace(version))
            {
                query = query.Where(x => x.Version == version);


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
                query = query.Include(x => x.ScriptConstraints.Where(x => x.Active));
            }

            if (modifiedDate != null)
            {
                query = query
                    .Where(x => x.ModifiedDate > modifiedDate || (x.ScriptConstraints != null && x.ScriptConstraints.Any(y => y.ModifiedDate > modifiedDate)));
            }

            return await query.ToListAsync();
        }

        public async Task<Script?> GetScriptByIdentifier(int? scriptId, (string name, string version)? primaryKey = null, bool includeConstraints = false, bool track = false)
        {
            IQueryable<Script> query = Entities;

            if (includeConstraints)
                query = query.Include(s => s.ScriptConstraints);

            if (!track)
                query = query.AsNoTracking();

            if (scriptId.HasValue)
                return await query.FirstOrDefaultAsync(s => s.ScriptId == scriptId.Value);


            return await query.FirstOrDefaultAsync();
        }

        public async Task<Script?> UpdateScript(Script script)
        {
            var entity = await Entities.FirstOrDefaultAsync(s => s.ScriptId == script.ScriptId);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Script with ID {script.ScriptId} not found.");
            }

            entity.Name = script.Name;
            entity.Version = script.Version;
            entity.Value = script.Value;
            entity.ModifiedBy = script.ModifiedBy;
            entity.Active = script.Active;

            // Update constraints if provided
            if (script.ScriptConstraints != null)
            {
                entity.ScriptConstraints = script.ScriptConstraints
                    .Select(sc => new ScriptConstraint
                    {
                        ScriptConstraintId = sc.ScriptConstraintId,
                        Branch = sc.Branch,
                        Name = sc.Name,
                        Version = sc.Version,
                        Constraint = sc.Constraint,
                        ModifiedBy = sc.ModifiedBy,
                        Active = sc.Active,
                        Script = entity
                    }).ToList();
            }
            return entity;
        }

        public async Task<Script?> AddScript(Script script)
        {
            var entity = new Script
            {
                ScriptId = script.ScriptId = 0,
                Branch = script.Branch,
                Name = script.Name,
                Version = script.Version,
                Value = script.Value,
                ModifiedBy = script.ModifiedBy,
                Active = script.Active,
                ScriptConstraints = script.ScriptConstraints?.Select(sc => new ScriptConstraint
                {
                    Name = sc.Name,
                    Branch = sc.Branch,
                    Version = sc.Version,
                    Constraint = sc.Constraint,
                    ModifiedBy = sc.ModifiedBy,
                    Active = sc.Active
                }).ToList()
            };

            await Entities.AddAsync(entity);
            return entity;
        }
    }

    async Task<IEnumerable<IScript>> GetScripts(string branch, string name, string version, bool includeConstraints, DateTime? modifiedDate)
        {
            IQueryable<Script> query = Scripts
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
                query = query.Include(x => x.ScriptConstraints != null);
            }

            if (modifiedDate != null)
            {
                query = query
                    .Where(x => x.ModifiedDate > modifiedDate || x.ScriptConstraints != null && x.ScriptConstraints.Any(y => y.ModifiedDate > modifiedDate));
            }

            return await query.ToListAsync();
        }
    public interface IScriptRepository : IRepository<Script>
    {
        DbSet<Script> Scripts { get; }
        Task<Script?> GetScriptByIdentifier(int? scriptId, (string name, string version)? primaryKey = null, bool includeConstraints = false, bool track = false);
        Task<Script?> UpdateScript(Script script);
        Task<Script?> AddScript(Script script);
        Task<IEnumerable<Script>?> GetScripts(string branch, string name, string version, bool includeConstraints, DateTime? modifiedDate);
    }

}
