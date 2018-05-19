using System.Collections.Generic;
using BizSoft.EventBus.Concretes;

namespace Catalog.WebApi.IntegrationEvents.Events
{
    public class OrderStockRejectedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public List<ConfirmedOrderStockItem> OrderStockItems { get; }

        public OrderStockRejectedIntegrationEvent( int orderId,
            List<ConfirmedOrderStockItem> orderStockItems )
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }
    }

    public class ConfirmedOrderStockItem
    {
        public int ProductId { get; }
        public bool HasStock { get; }

        public ConfirmedOrderStockItem( int productId, bool hasStock )
        {
            ProductId = productId;
            HasStock = hasStock;
        }
    }
}
