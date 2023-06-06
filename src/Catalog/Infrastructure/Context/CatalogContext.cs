using Catalog.Domain;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Infrastructure.Context
{
    public class CatalogContext : DbContext, IDbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) 
        : base(options) { }

        public const string DefaultSchema = "catalog";

        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }        
    }
}
