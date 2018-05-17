using System;
using System.Collections.Generic;
using System.Linq;
using BizSoft.Ordering.Core.AggregateEntities.Address;
using BizSoft.Ordering.Core.Events;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.Core.Entities.Order
{
    public class Order : Entity, IAggregateRoot
    {
        private int? _buyerId;
        public int? GetBuyerId => _buyerId;

        private DateTime _orderDate;

        private readonly List<OrderItem.OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem.OrderItem> OrderItems => _orderItems;
        
        public OrderStatus.OrderStatus OrderStatus { get; private set; }
        // Address is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public Address Address { get; private set; }

        protected Order(Address address)
        {
           _orderItems = new List<OrderItem.OrderItem>();
        }

        public Order
        (
            string userId,
            int? buyerId, 
            Address address
        ) 
            : this(address)
        {
            _orderDate = DateTime.UtcNow;
            _buyerId = buyerId;
            Address = address;

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

