using Microsoft.EntityFrameworkCore;
using React_dotnet.database.Models;

namespace React_dotnet.database.Configurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}
