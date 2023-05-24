using Microsoft.EntityFrameworkCore;
using TestForDotNet.Models;

namespace TestForDotNet.DataAccess
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
        }

        public DbSet<MonsterModel> Monsters { get; set; }
    }
}
