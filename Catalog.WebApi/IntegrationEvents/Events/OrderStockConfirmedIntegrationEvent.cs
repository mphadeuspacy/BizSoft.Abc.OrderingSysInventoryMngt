using BizSoft.EventBus.Concretes;

namespace Catalog.WebApi.IntegrationEvents.Events
{
    public class OrderStockConfirmedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public OrderStockConfirmedIntegrationEvent( int orderId ) => OrderId = orderId;
    }
}
