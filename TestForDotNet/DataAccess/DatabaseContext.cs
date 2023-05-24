using Microsoft.EntityFrameworkCore;
using TestForDotNet.Models;

namespace TestForDotNet.DataAccess
{
    public class DatabaseContext: DbContext
    {
        private readonly Action<DatabaseContext, ModelBuilder> _modelCustomizer;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, Action<DatabaseContext, ModelBuilder> modelCustomizer = null) : base(options)
        {
            _modelCustomizer = modelCustomizer;
        }

        public virtual DbSet<MonsterModel> Monsters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(_modelCustomizer is not null) {
                _modelCustomizer(this, modelBuilder);
            }
        }
    }
}
