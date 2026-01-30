using Microsoft.EntityFrameworkCore;
using React_dotnet.database.Models;
using System.Reflection;

namespace React_dotnet.database
{
    public class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
