using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using SharedScriptsApi.Enums;
using SharedScriptsApi.Extensions;

namespace SharedScriptsApi.Data
{

    [JsonObject("ConnectionStrings")]
    public class DbConnectionModels
    {
        [JsonProperty("DefaultConnection")]
        public DefaultConnection? DefaultConnection { get; set; }

        [JsonProperty("CoreConnection")]
        public CoreConnection? CoreConnection { get; set; }
        [JsonProperty("StateConnection")]
        public StateConnection? StateConnection { get; set; }
        public List<IConnectionDetail?> GetAllConnections() => 
            new List<IConnectionDetail?> { DefaultConnection, CoreConnection, StateConnection }.Where(c => c != null).ToList();

    }

    public class CoreConnection : ConnectionDetailBase, IConnectionDetail 
    {
    }
    public class DefaultConnection : ConnectionDetailBase, IConnectionDetail 
    {
    }

    public class StateConnection : ConnectionDetailBase, IConnectionDetail 
    {
    }

    public class ConnectionDetailBase
    {
        private string? _connectionString = null;
        public string GetConnectionString(string customer, string environment)
        {

            if (_connectionString == null)
            {
                _connectionString = this.Format
                    .Replace("{Domain}", this.Domain)
                    .Replace("{Database}", this.Database)
                    .Replace("{DatabaseUser}", this.DatabaseUser)
                    .Replace("{DatabasePassword}", this.DatabasePassword);
            }
            return _connectionString;
        }
        public virtual string GetName() => this.DbConnectionType.GetName();
        [JsonProperty("DbConnectionType")]
        [JsonConverter(typeof(StringToEnumConverter<DbConnectionType>))]
        public required DbConnectionType DbConnectionType { get; set; }

        [JsonProperty("Format")]
        public required string Format { get; set; }

        [JsonProperty("Domain")]
        public required string Domain { get; set; }

        [JsonProperty("Database")]
        public required string Database { get; set; }

        [JsonProperty("DatabaseUser")]
        public required string DatabaseUser { get; set; }

        [JsonProperty("DatabasePassword")]
        public required string DatabasePassword { get; set; }
    }


    public interface IConnectionDetail
    {
        [JsonIgnore]
        DbConnectionType DbConnectionType { get; }
        string GetName();

        [JsonProperty("Format")]
        string Format { get; set; }

        [JsonProperty("Domain")]
        string Domain { get; set; }

        [JsonProperty("Database")]
        string Database { get; set; }

        [JsonProperty("DatabaseUser")]
        string DatabaseUser { get; set; }

        [JsonProperty("DatabasePassword")]
        string DatabasePassword { get; set; }
    }
}
