namespace SharedScriptsApi.Interfaces
{
    namespace Saltus.digiTICKET.ExternalSources.Models
    {
        public interface IEntity
        {
            int? ModifiedBy { get; set; }
            DateTime ModifiedDate { get; set; }
            bool Active { get; set; }
            int GetId();
            object[] GetPrimaryKey();
        }
    }

}
