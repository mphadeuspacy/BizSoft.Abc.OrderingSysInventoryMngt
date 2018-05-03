using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MediatR;
using Ordering.WebApi.Commands.Abstracts;
using Ordering.WebApi.Extensions;
using Ordering.WebApi.Models;

namespace Ordering.WebApi.Commands.Concretes
{
    [DataContract]
    public class CreateOrderCommand : ICommand, IRequest<bool>
    {
        [DataMember] private readonly IList<OrderItemDto> _orderItems;
        [DataMember] public string UserId { get; private set; }
        [DataMember] public IEnumerable<OrderItemDto> OrderItems => _orderItems;

        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDto>();
        }

        public CreateOrderCommand
        (
            IList<BasketItem> basketItems,
            string userId
        ) : this()
        {
            _orderItems = basketItems.ToOrderItemsDto().ToList();
            UserId = userId;
        }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int NumberOfItems { get; set; }
        public string ImageUri { get; set;  }
    }
}
