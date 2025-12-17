using Microsoft.EntityFrameworkCore;

namespace SharedScriptsApi.Data
{
    public class DataContext : DbContext
    {
        private readonly IRepository _repo;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(DbContextOptions<DataContext> options, IRepository repository)
        {
            _repo = repository;
        }
    }
}
