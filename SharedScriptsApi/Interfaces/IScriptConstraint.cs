namespace SharedScriptsApi.Interfaces
{
    public interface IScriptConstraint
    {
        int ScriptConstraintId { get; set; }
        string Branch { get; set; } 
        string Name { get; set; }
        string Version { get; set; }
        string Constraint { get; set; }
        int? ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
        bool Active { get; set; }
        IScript Script { get; set; }
    }
}
