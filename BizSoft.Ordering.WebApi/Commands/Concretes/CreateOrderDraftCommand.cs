using System.Collections.Generic;
using BizSoft.Ordering.WebApi.Models;
using MediatR;
using Ordering.WebApi.Commands.Abstracts;
using Ordering.WebApi.Models;

namespace BizSoft.Ordering.WebApi.Commands.Concretes
{
    public class CreateOrderDraftCommand : ICommand, IRequest<OrderDraftDto>
    {

        public string BuyerId { get; }

        public IEnumerable<BasketItem> Items { get; }

        public CreateOrderDraftCommand( string buyerId, IEnumerable<BasketItem> items )
        {
            BuyerId = buyerId;
            Items = items;
        }
    }
}
