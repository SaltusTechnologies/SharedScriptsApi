using SharedScriptsApi.DataModels;

namespace SharedScriptsApi.Interfaces
{
    public interface IScriptService
    {
        Task<IEnumerable<IScript>?> GetScriptsAsync();
        Task<IEnumerable<IScript>?> GetScriptsAsync(string version, bool includeConstraints, Dictionary<string, string> branchOverrides, DateTime modifiedDate);
        Task<IEnumerable<IScript>?> GetScriptsAsync(string customer, string environment, string branch, string version, bool includeConstraints, DateTime? modifiedDate);
        Task<IEnumerable<IScript>?> GetScriptsAsync(string branch, string name, string version, bool includeConstraints, DateTime? modifiedDate);
        Task<IScript?> GetScriptAsync(int scriptId);
        Task AddScriptAsync(IScript script);
        Task UpdateScriptAsync(IScript script);
    }
}