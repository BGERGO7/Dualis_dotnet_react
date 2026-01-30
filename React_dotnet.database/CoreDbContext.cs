using Microsoft.EntityFrameworkCore;
using React_dotnet.database.Models;
using System.Reflection;

namespace React_dotnet.database
{
    public class CoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
