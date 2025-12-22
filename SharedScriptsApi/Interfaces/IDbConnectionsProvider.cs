

using SharedScriptsApi.Data;
using SharedScriptsApi.Enums;

namespace SharedScriptsApi.Interfaces
{
    public interface IDbConnectionsProvider
    {
        DbConnectionModels DbConnectionModels { get; }
        IConnectionDetail? GetConnectionDetail(string typeName);
        IConnectionDetail? GetConnectionDetail(DbConnectionType dbConnectionType);
        string GetConnectionString(DbConnectionType dbConnectionType);
        string GetConnectionString(string typeName);
    }
}
