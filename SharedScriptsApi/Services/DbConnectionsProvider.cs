using SharedScriptsApi.Data;
using SharedScriptsApi.Enums;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Services
{
    public class DbConnectionsProvider : IDbConnectionsProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ICryptographyProvider _cryptographyProvider;
        public DbConnectionModels DbConnectionModels { get; set; } = new();

        public DbConnectionsProvider(IConfiguration configuration, IServiceProvider serviceProvider, ICryptographyProvider cryptographyProvider)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var section = this._configuration.GetSection("ConnectionStrings");
            section.Bind(this.DbConnectionModels);

            foreach (var dbConnection in this.DbConnectionModels.GetAllConnections())
            {
                if (dbConnection == null) continue;
                var decryptedPassword = _cryptographyProvider?.Decrypt(dbConnection.DatabasePassword);
                if (string.IsNullOrWhiteSpace(decryptedPassword))
                {
                    throw new InvalidOperationException($"Decrypted password for {dbConnection.GetName()} is null or empty.");
                }
            }

            _serviceProvider = serviceProvider;
            _cryptographyProvider = cryptographyProvider;
        }

        public IConnectionDetail? GetConnectionDetail(string typeName)
        {
            return this.DbConnectionModels.GetConnectionDetail(Enum.Parse<DbConnectionType>(typeName));
        }

        public IConnectionDetail? GetConnectionDetail(DbConnectionType dbConnectionType)
        {
            return this.DbConnectionModels.GetConnectionDetail(dbConnectionType);
        }

        public string GetConnectionString(DbConnectionType dbConnectionType)
        {
            var connectionDetail = this.GetConnectionDetail(dbConnectionType);
            if (connectionDetail == null) throw new ArgumentNullException(nameof(connectionDetail));
            var decryptedPassword = this._cryptographyProvider.Decrypt(connectionDetail.DatabasePassword);
            return string.Format(connectionDetail.Format,
                                 connectionDetail.Domain,
                                 connectionDetail.Database,
                                 connectionDetail.DatabaseUser,
                                 decryptedPassword);
        }

        public string GetConnectionString(string typeName)
        {
            var connectionDetail = GetConnectionDetail(typeName);
            if (connectionDetail == null) throw new ArgumentNullException(nameof(connectionDetail));

            var decryptedPassword = _cryptographyProvider.Decrypt(connectionDetail.DatabasePassword);

            return string.Format(connectionDetail.Format,
                                 connectionDetail.Domain,
                                 connectionDetail.Database,
                                 connectionDetail.DatabaseUser,
                                 decryptedPassword);
        }
    }
}
