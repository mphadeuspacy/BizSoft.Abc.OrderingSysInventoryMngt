using BizSoft.Ordering.Core.Entities.OrderItem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizSoft.Ordering.EntityFrameworkCore.Configurations
{
    class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItemEntityTypeBuilder)
        {
            orderItemEntityTypeBuilder.ToTable( "orderItems", OrderingDbContext.DEFAULT_SCHEMA );

            orderItemEntityTypeBuilder.HasKey( o => o.Id );

            orderItemEntityTypeBuilder.Ignore( b => b.DomainEvents );

            orderItemEntityTypeBuilder.Property( o => o.Id ).ForSqlServerUseSequenceHiLo( "orderitemseq" );

            orderItemEntityTypeBuilder.Property<int>( "OrderId" ).IsRequired();

            orderItemEntityTypeBuilder.Property<int>( "ProductId" ).IsRequired();

            orderItemEntityTypeBuilder.Property<string>( "ProductName" ).IsRequired();

            orderItemEntityTypeBuilder.Property<decimal>( "Price" ).IsRequired();

            orderItemEntityTypeBuilder.Property<int>( "NumberOfUnits" ).IsRequired();

            orderItemEntityTypeBuilder.Property<string>( "ImageUriUri" ).IsRequired( false );
        }
    }
}
