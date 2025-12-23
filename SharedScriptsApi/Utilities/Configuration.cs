using Microsoft.Extensions.DependencyInjection;
using SharedScriptsApi.Interfaces;

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
