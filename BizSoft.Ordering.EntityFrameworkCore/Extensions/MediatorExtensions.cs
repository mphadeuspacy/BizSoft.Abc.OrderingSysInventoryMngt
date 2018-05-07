using System.Linq;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using MediatR;

namespace BizSoft.Ordering.EntityFrameworkCore.Extensions
{
    internal static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingDbContext orderingDbContext)
        {
            var domainEntities = orderingDbContext
                .ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any() == true)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(e => e.Entity.ResetDomainEvent());

            var tasks = domainEvents
                .Select(async domainEvent =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
