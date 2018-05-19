using Catalog.WebApi.Configurations;
using Catalog.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.WebApi.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext( DbContextOptions<CatalogDbContext> options ) : base( options ){}

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            builder.ApplyConfiguration( new CatalogBrandEntityTypeConfiguration() );
            builder.ApplyConfiguration( new CatalogTypeEntityTypeConfiguration() );
            builder.ApplyConfiguration( new CatalogItemEntityTypeConfiguration() );
        }
    }
}
