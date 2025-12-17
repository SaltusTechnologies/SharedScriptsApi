
namespace SharedScriptsApi.Interfaces
{
    public interface IScript
    {
        int ScriptId { get; set; }
        string Branch { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string? Value { get; set; }
        int? ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
        bool Active { get; set; }
        IEnumerable<IScriptConstraint>? ScriptConstraints { get; set; }
    }
}
