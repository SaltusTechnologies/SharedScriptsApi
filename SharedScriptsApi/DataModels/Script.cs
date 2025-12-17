using Saltus.digiTICKET.Data0111000000.Models;
using SharedScriptsApi.Interfaces;
using System.Data.SqlTypes;

namespace SharedScriptsApi.DataModels
{
    [Serializable]
    public class Script : IScript
    {
        public int ScriptId { get; set; }
        public required string Branch { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public string? Value { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = SqlDateTime.MinValue.Value;
        public bool Active { get; set; }
        public List<ScriptConstraint>? ScriptConstraints { get; set; }
        IEnumerable<IScriptConstraint>? IScript.ScriptConstraints { get => ScriptConstraints; set => ScriptConstraints = (List<ScriptConstraint>?)value; }
    }
}
