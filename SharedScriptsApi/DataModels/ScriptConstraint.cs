using SharedScriptsApi.DataModels;
using SharedScriptsApi.Interfaces;
using SharedScriptsApi.Interfaces.Saltus.digiTICKET.ExternalSources.Models;
using System;
using System.Data.SqlTypes;

namespace Saltus.digiTICKET.Data0111000000.Models
{
    [Serializable]
    public class ScriptConstraint : IScriptConstraint, IEntity
    {
        public int ScriptConstraintId { get; set; }
        public required string Branch { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required string Constraint { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = SqlDateTime.MinValue.Value;
        public bool Active { get; set; }
        public Script Script { get; set; }
        IScript IScriptConstraint.Script { get => Script; set => Script = (Script)value; }

        public int GetId()
        {
            throw new NotImplementedException();
        }

        public object[] GetPrimaryKey()
        {
            throw new NotImplementedException();
        }
    }
}
