using System.Collections.Generic;
using MediatR;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    /// <summary>
    /// This calls implements an Observer pattern by acting as a storage of domain events
    /// to be published on any state entity changes that are persisted.
    /// </summary>
    public abstract class Entity
    {
        public virtual int Id { get; protected set; }

        //TODO: Consider using a Set instead to guarantee no duplicate event collection
        private List<INotification> _domainEvents;

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void SubscribeDomainEvent(INotification domainEventNotification )
        {
            _domainEvents = _domainEvents ?? new List<INotification>();

            _domainEvents.Add( domainEventNotification );
        }

        public void UnsubscribeDomainEvent(INotification domainEventNotification)
        {
            _domainEvents?.Remove(domainEventNotification);
        }

        public void ResetDomainEvent()
        {
            _domainEvents?.Clear();
        }
    }
}
