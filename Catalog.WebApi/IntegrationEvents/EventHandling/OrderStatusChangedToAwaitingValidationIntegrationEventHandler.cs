using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizSoft.EventBus.Abstracts;
using BizSoft.EventBus.Concretes;
using Catalog.WebApi.Infrastructure;
using Catalog.WebApi.IntegrationEvents.Events;

namespace Catalog.WebApi.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
        IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly CatalogDbContext _catalogContext;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler( CatalogDbContext catalogContext,
            ICatalogIntegrationEventService catalogIntegrationEventService )
        {
            _catalogContext = catalogContext;
            _catalogIntegrationEventService = catalogIntegrationEventService;
        }

        public async Task Handle( OrderStatusChangedToAwaitingValidationIntegrationEvent command )
        {
            var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

            foreach (var orderStockItem in command.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find( orderStockItem.ProductId );

                var hasStock = catalogItem.AvailableStock >= orderStockItem.NumberOfUnits;

                var confirmedOrderStockItem = new ConfirmedOrderStockItem( catalogItem.Id, hasStock );

                confirmedOrderStockItems.Add( confirmedOrderStockItem );
            }

            var confirmedIntegrationEvent = confirmedOrderStockItems.Any( c => !c.HasStock )
                ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent( command.OrderId, confirmedOrderStockItems )
                : new OrderStockConfirmedIntegrationEvent( command.OrderId );

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync( confirmedIntegrationEvent );

            await _catalogIntegrationEventService.PublishThroughEventBusAsync( confirmedIntegrationEvent );
        }
    }
}
