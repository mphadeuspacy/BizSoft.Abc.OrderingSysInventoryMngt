﻿using Catalog.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.WebApi.Configurations
{
    class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure( EntityTypeBuilder<CatalogItem> builder )
        {
            builder.ToTable( "Catalog" );

            builder.Property( ci => ci.Id ).ForSqlServerUseSequenceHiLo( "catalog_hilo" ).IsRequired();

            builder.Property( ci => ci.Name ).IsRequired( true ).HasMaxLength( 50 );

            builder.Property( ci => ci.Price ).IsRequired( true );

            builder.Property( ci => ci.ImageFileName ).IsRequired( false );

            builder.Ignore( ci => ci.ImageUri );

            builder.HasOne( ci => ci.CatalogBrand ).WithMany().HasForeignKey( ci => ci.CatalogBrandId );

            builder.HasOne( ci => ci.CatalogType ).WithMany().HasForeignKey( ci => ci.CatalogTypeId );
        }
    }
}
