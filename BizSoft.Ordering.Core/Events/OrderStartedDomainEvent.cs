using BizSoft.Ordering.Core.Entities.Order;
using MediatR;

namespace BizSoft.Ordering.Core.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public string UserId { get; }
        public Order Order { get; }

        public OrderStartedDomainEvent(Order order, string userId)
        {
            Order = order;
            UserId = userId;
        }
    }
}
