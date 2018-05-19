using System;
using System.Data.Common;
using System.Threading.Tasks;
using BizSoft.EventBus.Abstracts;
using BizSoft.EventBus.Concretes;
using BizSoft.IntegrationEventLogEf;
using BizSoft.IntegrationEventLogEf.Services.Abstracts;
using Catalog.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.WebApi.IntegrationEvents
{
    public class CatalogIntegrationEventService : ICatalogIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly CatalogDbContext _catalogDbContext;
        private readonly IIntegrationEventLogService _eventLogService;

        public CatalogIntegrationEventService( IEventBus eventBus, CatalogDbContext catalogDbContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory )
        {
            _catalogDbContext = catalogDbContext ?? throw new ArgumentNullException( nameof( catalogDbContext ) );
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException( nameof( integrationEventLogServiceFactory ) );
            _eventBus = eventBus ?? throw new ArgumentNullException( nameof( eventBus ) );
            _eventLogService = _integrationEventLogServiceFactory( _catalogDbContext.Database.GetDbConnection() );
        }

        public async Task PublishThroughEventBusAsync( IntegrationEvent integrationEvent )
        {
            _eventBus.Publish( integrationEvent );

            await _eventLogService.MarkEventAsPublishedAsync( integrationEvent );
        }

        public async Task SaveEventAndCatalogContextChangesAsync( IntegrationEvent integrationEvent )
        {
            await ResilientTransaction.New( _catalogDbContext )
                .ExecuteAsync( async () => {
                    
                    await _catalogDbContext.SaveChangesAsync();
                    await _eventLogService.SaveEventAsync( integrationEvent, _catalogDbContext.Database.CurrentTransaction.GetDbTransaction() );
                } );
        }
    }

    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync( IntegrationEvent integrationEvent );
        Task PublishThroughEventBusAsync( IntegrationEvent integrationEvent );
    }
}
