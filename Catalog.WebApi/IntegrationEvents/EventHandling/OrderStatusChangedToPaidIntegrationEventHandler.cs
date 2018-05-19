using System.Threading.Tasks;
using BizSoft.EventBus.Abstracts;
using Catalog.WebApi.Infrastructure;

namespace Catalog.WebApi.IntegrationEvents.Events
{
    public class OrderStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly CatalogDbContext _catalogContext;

        public OrderStatusChangedToPaidIntegrationEventHandler( CatalogDbContext catalogContext )
        {
            _catalogContext = catalogContext;
        }

        public async Task Handle( OrderStatusChangedToPaidIntegrationEvent command )
        {
            foreach (var orderStockItem in command.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find( orderStockItem.ProductId );

                catalogItem.RemoveStock( orderStockItem.NumberOfUnits );
            }

            await _catalogContext.SaveChangesAsync();
        }
    }
}
