using BizSoft.Ordering.Core.Entities.OrderStatus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizSoft.Ordering.EntityFrameworkCore.Configurations
{
    class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure( EntityTypeBuilder<OrderStatus> orderStatusConfiguration )
        {
            orderStatusConfiguration.ToTable( "orderstatus", OrderingDbContext.DEFAULT_SCHEMA );

            orderStatusConfiguration.HasKey( o => o.Id );

            orderStatusConfiguration.Property( o => o.Id ).HasDefaultValue( 1 ).ValueGeneratedNever().IsRequired();

            orderStatusConfiguration.Property( o => o.Name ).HasMaxLength( 200 ).IsRequired();
        }
    }
}
