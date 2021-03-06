﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BizSoft.Ordering.WebApi.Extensions;
using BizSoft.Ordering.WebApi.Models;
using MediatR;
using Ordering.WebApi.Commands.Abstracts;
using Ordering.WebApi.Models;

namespace Ordering.WebApi.Commands.Concretes
{
    [DataContract]
    public class CreateOrderCommand : ICommand, IRequest<bool>
    {
        [DataMember] private readonly IList<OrderItemDto> _orderItems;
        [DataMember] public string UserId { get; private set; }
        [DataMember] public IEnumerable<OrderItemDto> OrderItems => _orderItems;
        [DataMember] public string Street { get; set; }
        [DataMember] public string City { get; set; }
        [DataMember] public string State { get; set; }
        [DataMember] public string Country { get; set; }
        [DataMember] public string ZipCode { get; set; }

        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDto>();
        }

        public CreateOrderCommand
        (
            IList<BasketItem> basketItems,
            string userId,
            string city, 
            string street, 
            string state, 
            string country, 
            string zipcode
        ) 
            : this()
        {
            _orderItems = basketItems.ToOrderItemsDto().ToList();
            UserId = userId;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }
    }
}
