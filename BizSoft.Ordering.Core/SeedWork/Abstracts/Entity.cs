using System.Collections.Generic;
using MediatR;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public class Entity
    {
        public virtual int Id { get; protected set; }

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
