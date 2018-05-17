using System;
using BizSoft.Ordering.Core.AggregateEntities.Buyer;
using BizSoft.Ordering.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizSoft.Ordering.EntityFrameworkCore.Configurations
{
    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderEndEntityTypeBuilder)
        {
            orderEndEntityTypeBuilder.ToTable( "orders", OrderingDbContext.DEFAULT_SCHEMA );

            orderEndEntityTypeBuilder.HasKey( o => o.Id );

            orderEndEntityTypeBuilder.Ignore( d => d.DomainEvents );

            orderEndEntityTypeBuilder.Property( o => o.Id )
                // The Hi/Lo algorithm as the key generation strategy. The Hi/Lo algorithm is useful when you need unique keys.
                // As a summary, the Hi-Lo algorithm assigns unique identifiers to table rows while not depending on storing the row in the database immediately.
                // This lets you start using the identifiers right away, as happens with regular sequential database IDs
                .ForSqlServerUseSequenceHiLo( "orderseq", OrderingDbContext.DEFAULT_SCHEMA );

            //Address value object persisted as owned entity type supported since EF Core 2.0
            orderEndEntityTypeBuilder.OwnsOne( o => o.Address );

            orderEndEntityTypeBuilder.Property<DateTime>( "OrderDate" ).IsRequired();
            orderEndEntityTypeBuilder.Property<int?>( "BuyerId" ).IsRequired( false );
            orderEndEntityTypeBuilder.Property<int>( "OrderStatusId" ).IsRequired();

            // Access the OrderItem collection property through its field
            orderEndEntityTypeBuilder.Metadata.FindNavigation( nameof( Order.OrderItems ) ).SetPropertyAccessMode( PropertyAccessMode.Field );

            orderEndEntityTypeBuilder.HasOne<Buyer>().WithMany().IsRequired( false ).HasForeignKey( "BuyerId" );

            orderEndEntityTypeBuilder.HasOne( o => o.OrderStatus ).WithMany().HasForeignKey( "OrderStatusId" );

        }
    }
}
