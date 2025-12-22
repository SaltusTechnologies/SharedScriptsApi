
using SharedScriptsApi.Data;
using SharedScriptsApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedScriptsApi.Extensions
{
    public static class DbConnectionExtensions
    {
        public static IConnectionDetail? GetConnectionDetail(this DbConnectionModels connectionModels, DbConnectionType type)
        {
            if (connectionModels == null) throw new ArgumentNullException(nameof(connectionModels));
            switch (type)
            {
                case DbConnectionType.Customer:
                    return connectionModels.DefaultConnection;
                case DbConnectionType.Core:
                    return connectionModels.CoreConnection;
                case DbConnectionType.Oklahoma:
                    return connectionModels.StateConnection;
                    // Assuming you have a StateConnection property in DbConnectionModels
                    // return connectionModels.StateConnection;
                    throw new NotImplementedException("State connection is not implemented.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported connection type: {type}");
            }
            ;
        }

        public static IConnectionDetail GetConnectionDetail(this DbConnectionModels connectionModels, string typeName)
        {
            if (connectionModels == null) throw new ArgumentNullException(nameof(connectionModels));
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("Connection type name must not be null or empty.", nameof(typeName));
            var connectionType = FromName(typeName);
            var connectionDetail = connectionModels.GetConnectionDetail(connectionType);
            return connectionDetail ?? throw new InvalidOperationException($"No connection detail found for type: {typeName}");
        }

        public static string GetName(this DbConnectionType enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? enumValue.ToString(); // Fallback to ToString() if no DisplayAttribute
        }

        public static DbConnectionType FromName(string displayName)
        {
            foreach (var field in typeof(DbConnectionType).GetFields())
            {
                var attribute = field.GetCustomAttribute<DisplayAttribute>();
                if (attribute != null && attribute.Name == displayName)
                {
                    return Enum.Parse<DbConnectionType>(field.Name);
                }
            }
            throw new ArgumentException($"No ScriptType found with display name '{displayName}'", nameof(displayName));
        }


    }
}
