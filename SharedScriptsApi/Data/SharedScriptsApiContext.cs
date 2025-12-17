using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedScriptsApi.DataModels;

namespace SharedScriptsApi.Data
{
    public class SharedScriptsApiContext : S
    {
        public SharedScriptsApiContext (DbContextOptions<SharedScriptsApiContext> options)
            : base(options)
        {
        }

        public DbSet<SharedScriptsApi.DataModels.Script> Script { get; set; } = default!;
    }
}
