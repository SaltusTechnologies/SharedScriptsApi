using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.Data;
using SharedScriptsApi.Interfaces.Saltus.digiTICKET.ExternalSources.Models;

namespace SharedScriptsApi.Extensions
{
    public static class IRepositoryExtensions
    {
        public static T? As<T>(this IRepository<IEntity> repository)
            where T : Repository<IEntity>, new()
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository), "The repository cannot be null.");
            }

            var entities = repository.Entities;

            if (entities is null)
            {
                throw new InvalidOperationException("The repository's DbSet cannot be null.");
            }

            var canCast = repository.TryCast(out T repo);
            return canCast ? repo : new T();
        }

        public static bool TryCast<T>(this object value, out T result)
        {
            bool retVal = false;
            if (value is T cast)
            {
                result = cast;
                retVal = true;
            }
            else
            {
                result = default;
            }
            return retVal;
        }
    }
}
