using Catalog.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.WebApi.Configurations
{
    class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure( EntityTypeBuilder<CatalogBrand> builder )
        {
            builder.ToTable( "CatalogBrand" );

            builder.HasKey( ci => ci.Id );

            builder.Property( ci => ci.Id ).ForSqlServerUseSequenceHiLo( "catalog_brand_hilo" ).IsRequired();

            builder.Property( cb => cb.Brand ).IsRequired().HasMaxLength( 100 );
        }
    }
}
