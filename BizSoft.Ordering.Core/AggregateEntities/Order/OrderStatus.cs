using System.Collections.Generic;
using BizSoft.Ordering.Core.SeedWork.Concretes;

namespace BizSoft.Ordering.Core.Entities.OrderStatus
{
    public class OrderStatus : Enumeration
    {
        public static readonly OrderStatus Submitted = new OrderStatus( 1, nameof( Submitted ).ToLowerInvariant() );
        public static readonly OrderStatus AwaitingValidation = new OrderStatus( 2, nameof( AwaitingValidation ).ToLowerInvariant() );
        public static readonly OrderStatus StockConfirmed = new OrderStatus( 3, nameof( StockConfirmed ).ToLowerInvariant() );
        public static readonly OrderStatus Paid = new OrderStatus( 4, nameof( Paid ).ToLowerInvariant() );
        public static readonly OrderStatus Shipped = new OrderStatus( 5, nameof( Shipped ).ToLowerInvariant() );
        public static readonly OrderStatus Cancelled = new OrderStatus( 6, nameof( Cancelled ).ToLowerInvariant() );

        public OrderStatus(int id, string name) : base(id, name) {}

        public static IEnumerable<OrderStatus> AllOrderStatuses() => new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };
    }
}
