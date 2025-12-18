using SharedScriptsApi.Interfaces;
using SharedScriptsApi.DataModels;

namespace SharedScriptsApi.Services
{
    public class ScriptService<T> : DataService<T>
        where T :Script
    {
        private readonly IServiceProvider _serviceProvider;
        public ScriptService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }


    }
}
