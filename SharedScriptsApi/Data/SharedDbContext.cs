using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.DataModels.ModelConfiguration;
using SharedScriptsApi.Interfaces;


namespace SharedScriptsApi.Data
{
    public class SharedDbContext<T> : DbContextBase, ISharedDbContext<T>
            where T : IConnectionDetail
    {
        private const string CONNECTION_TYPE_NAME_DEFAULT = "Customer";
        private const string CONNECTION_TYPE_NAME_CORE = "Core";
        private readonly IDbConnectionsProvider _connectionProvider;
        private T? _connectionDetail;
        private string? _tableName;
        public override DbSet<TEntity> Entity<TEntity>() where TEntity : class => Set<TEntity>();

        public SharedDbContext(IDbConnectionsProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            switch (typeof(T))
            {
                case Type t when t == typeof(DefaultConnection):
                    _connectionDetail = (T)_connectionProvider.GetConnectionDetail(CONNECTION_TYPE_NAME_DEFAULT)!;
                    break;
                    case Type t when t == typeof(CoreConnection):
                    _connectionDetail = (T)_connectionProvider.GetConnectionDetail(CONNECTION_TYPE_NAME_CORE)!;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported connection type: {typeof(T).Name}");
            }
        }

        public SharedDbContext(IDbConnectionsProvider connectionProvider, T connectionDetail, string tableName)
            : this(connectionProvider)
        {
            _connectionDetail = connectionDetail ?? throw new ArgumentNullException(nameof(connectionDetail));
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_connectionDetail == null)
                {
                    throw new InvalidOperationException("Connection detail is not configured.");
                }

                optionsBuilder.UseSqlServer(_connectionProvider.GetConnectionString(_connectionDetail.DbConnectionType));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ScriptConfiguration().Configure(modelBuilder.Entity<Script>().ToTable($"{_connectionDetail!.GetName()}_{nameof(Script)}",  tb => tb.HasTrigger("LogScriptChanges")));
            new ScriptConstraintConfiguration().Configure(modelBuilder.Entity<ScriptConstraint>().ToTable($"{_connectionDetail!.GetName()}_{nameof(ScriptConstraint)}"));
        }
    }
}
