using Microsoft.Extensions.DependencyInjection;
using Saltus.digiTICKET.DataInterfaces;

namespace SharedScriptsApi.Utilities
{
    public static class Configuration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<ICryptographyProvider, CryptographyProvider>();
        }
    }
}
