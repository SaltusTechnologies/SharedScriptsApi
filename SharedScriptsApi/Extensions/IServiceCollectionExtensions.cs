using SharedScriptsApi.Data;
using SharedScriptsApi.Enums;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Interfaces;
using SharedScriptsApi.Services;

namespace SharedScriptsApi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the external sources services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddExternalSourcesServices(this IServiceCollection services)
        {
            services.AddKeyedScoped<IUnitOfWork>(DbConnectionType.Core, (provider, _) => 
            {
                var dbConnectionsProvider = provider.GetRequiredService<IDbConnectionsProvider>();
                var connection = dbConnectionsProvider.GetConnectionDetail(DbConnectionType.Core);
                //var configProvider = provider.GetRequiredService<IAppConfigProvider>();
                return new CoreUnitOfWorkBase(new SharedDbContext<CoreConnection>(dbConnectionsProvider));
            });

            services.AddKeyedScoped<IUnitOfWork, CustomerUnitOfWorkBase>(DbConnectionType.Customer);
            services.AddScoped(typeof(ISharedDbContext<>), typeof(SharedDbContext<>));
            services.AddTransient<IDbConnectionsProvider, DbConnectionsProvider>();
            return services;
        }
    }
}
