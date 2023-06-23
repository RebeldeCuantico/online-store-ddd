using Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Common.Domain;

namespace Catalog.Infrastructure.Context.Mapping
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product), CatalogContext.DefaultSchema);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(x => x.Value, l => new EntityId(l));
            builder.OwnsOne(c => c.Name).Property(p => p.Value).HasColumnName("Name").IsRequired();
            builder.OwnsOne(c => c.Description).Property(p => p.Value).HasColumnName("Description").IsRequired();
            builder.OwnsOne(c => c.AvailableStock).Property(p => p.Value).HasColumnName("Stock").IsRequired();
            builder.OwnsOne(c => c.Price).Property(p => p.Value).HasColumnName("Price").IsRequired();
            builder.OwnsOne(c => c.CategoryId).Property(p => p.Value).HasColumnName("CategoryId").IsRequired();
            builder.HasAlternateKey(c => c.ProductCode);
            builder.Property(c => c.ProductCode).HasConversion(x => x.Value, l => new ProductCode(l)).HasColumnName("Code").IsRequired();
            //builder.OwnsOne(c => c.ProductCode).Property(p => p.Value).HasColumnName("ProductCode").IsRequired();                        
            builder.Ignore(c => c.DomainEvents);
            builder.Property<DateTime>("CreateDate");
            builder.Property<DateTime>("UpdateDate");
            builder.Property<DateTime?>("DeleteDate");
        }

    }
}
