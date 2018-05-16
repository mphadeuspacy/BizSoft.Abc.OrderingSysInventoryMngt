using System;
using BizSoft.Ordering.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizSoft.Ordering.EntityFrameworkCore.Configurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderEndEntityTypeBuilder)
        {
            orderEndEntityTypeBuilder
                .ToTable( "orders", OrderingDbContext.DEFAULT_SCHEMA );

            orderEndEntityTypeBuilder
                .HasKey( o => o.Id );

            orderEndEntityTypeBuilder
                .Ignore( b => b.DomainEvents );

            orderEndEntityTypeBuilder
                .Property( o => o.Id )
                // The Hi/Lo algorithm as the key generation strategy. The Hi/Lo algorithm is useful when you need unique keys.
                // As a summary, the Hi-Lo algorithm assigns unique identifiers to table rows while not depending on storing the row in the database immediately.
                // This lets you start using the identifiers right away, as happens with regular sequential database IDs
                .ForSqlServerUseSequenceHiLo( "orderseq", OrderingDbContext.DEFAULT_SCHEMA ); throw new NotImplementedException();
        }
    }
}
