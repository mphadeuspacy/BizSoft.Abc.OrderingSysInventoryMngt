using BizSoft.Ordering.Core.AggregateEntities.Buyer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizSoft.Ordering.EntityFrameworkCore.Configurations
{
    class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure( EntityTypeBuilder<Buyer> buyerConfiguration )
        {
            buyerConfiguration.ToTable( "buyers", OrderingDbContext.DEFAULT_SCHEMA );

            buyerConfiguration.HasKey( b => b.Id );

            buyerConfiguration.Ignore( b => b.DomainEvents );

            buyerConfiguration.Property( b => b.Id ).ForSqlServerUseSequenceHiLo( "buyerseq", OrderingDbContext.DEFAULT_SCHEMA );

            buyerConfiguration.Property( b => b.IdentityGuid ).HasMaxLength( 200 ).IsRequired();

            buyerConfiguration.HasIndex( "IdentityGuid" ).IsUnique( true );

            buyerConfiguration.Property( b => b.Name );
        }
    }
}

