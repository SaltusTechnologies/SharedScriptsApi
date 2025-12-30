using SharedScriptsApi.Interfaces;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Data;
using SharedScriptsApi.Enums;
using Newtonsoft.Json.Linq;

namespace SharedScriptsApi.Services
{
    public class ScriptService : IScriptService
    {
        private readonly IServiceProvider _serviceProvider;
        public ScriptService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IServiceResponse<List<Script>?>?> GetScriptsAsync(JToken token, Guid id, DbConnectionType connectionType) 
        {
            IServiceResponse<List<Script>?>? response = null;
            var unitOfWork = _serviceProvider.GetKeyedService<IUnitOfWork>(connectionType);
            unitOfWork.SetTableName("whatever");
            var data = unitOfWork.Get<IScriptRepository>().GetScripts();
            return response;
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
            await _scriptRepository.AddAsync((Script)script);
        }

        public async Task UpdateScriptAsync(IScript script)
        {
            await _scriptRepository.UpdateAsync((Script)script);
        }
    }
}
