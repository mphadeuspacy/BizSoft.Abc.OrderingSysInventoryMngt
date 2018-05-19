using System.Collections.Generic;
using BizSoft.EventBus.Concretes;
using Catalog.WebApi.Dtos;

namespace Catalog.WebApi.IntegrationEvents.Events
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }
        public IEnumerable<OrderStockItemDto> OrderStockItems { get; }

        public OrderStatusChangedToAwaitingValidationIntegrationEvent( int orderId,
            IEnumerable<OrderStockItemDto> orderStockItems )
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }
    }
}
