using System.Collections.Generic;
using BizSoft.EventBus.Concretes;
using Catalog.WebApi.Dtos;

namespace Catalog.WebApi.IntegrationEvents.Events
{
    public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }
        public IEnumerable<OrderStockItemDto> OrderStockItems { get; }

        public OrderStatusChangedToPaidIntegrationEvent( int orderId,
            IEnumerable<OrderStockItemDto> orderStockItems )
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }
    }
}
