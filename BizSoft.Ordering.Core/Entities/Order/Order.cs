using System.Collections.Generic;
using System.Linq;
using BizSoft.Ordering.Core.Events;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.Core.Entities.Order
{
    public class Order : Entity, IEntityRoot
    {
        private readonly List<OrderItem.OrderItem> _orderItems;
        private int? _buyerId;

        public IReadOnlyCollection<OrderItem.OrderItem> OrderItems => _orderItems;
        
        public OrderStatus OrderStatus { get; private set; }

        protected Order()
        {
            _orderItems = new List<OrderItem.OrderItem>();
        }

        public Order
        (
            string userId,
            int? buyerId
        ) 
            : this()
        {
            _buyerId = buyerId;
            SubscribeDomainEvent(new OrderStartedDomainEvent( this, userId));
        }

        public void AddOrderItem(int productId, string productName, decimal price, string imageUri, int numberOfItems)
        {
            OrderItem.OrderItem exitsingOrderItemForProduct = _orderItems.SingleOrDefault(orderItem => orderItem.ProductId == productId);

            if (exitsingOrderItemForProduct != null)
            {
                exitsingOrderItemForProduct.AddOrderItems(numberOfItems);
            }
            else
            {
                _orderItems.Add(new OrderItem.OrderItem(productId, productName, price, imageUri, numberOfItems));
            }
        }
    }
}

