using SharedScriptsApi.Interfaces;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Data;
using static SharedScriptsApi.Data.ScriptRepository;

namespace SharedScriptsApi.Services
{
    public class ScriptService<T> : DataService<T>
        where T :Script
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScriptRepository _scriptRepository;
        public ScriptService(IServiceProvider serviceProvider, IScriptRepository scriptRepository) 
        {
            _serviceProvider = serviceProvider;
            _scriptRepository = scriptRepository;
        }


        public async  Task<IEnumerable<IScript>?> GetScriptsAsync() 
        {
            return await _scriptRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IScript>?> GetScriptsAsync(string version, bool includeConstraints, Dictionary<string, string> branchOverrides, DateTime ModifiedDate)
        {
            return await _scriptRepository.GetScripts(version, includeConstraints, branchOverrides, ModifiedDate);
        }

        public async Task<IEnumerable<IScript>?> GetScriptsAsync(string customer, string environment, string branch, string version, bool includeConstraints, DateTime? modifiedDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IScript>?> GetScriptsAsync(string branch, string name, string version, bool includeConstraints, DateTime? modifiedDate)
        {
            return await _scriptRepository.GetScripts(branch, name, version, includeConstraints, modifiedDate);
        }

        public async Task<IScript?> GetScriptAsync(int scriptId)
        {
            return await _scriptRepository.GetAsync(scriptId, false);
        }

        public async Task AddScriptAsync(IScript script)
        {
            await _scriptRepository.AddAsync((T)script);
        }

        public async Task UpdateScriptAsync(IScript script)
        {
            await _scriptRepository.UpdateAsync((T)script);
        }
    }
}
