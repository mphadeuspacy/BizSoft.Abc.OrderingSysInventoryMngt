using System.Collections.Generic;
using System.Linq;
using BizSoft.Ordering.WebApi.Models;
using Ordering.WebApi.Models;

namespace BizSoft.Ordering.WebApi.Extensions
{
    public static class BasketItemExtensions
    {
        public static IEnumerable<OrderItemDto> ToOrderItemsDto( this IEnumerable<BasketItem> basketItems )
        {
            return basketItems.Select(item => item.ToOrderItemDto());
        }

        public static OrderItemDto ToOrderItemDto( this BasketItem item )
        {
            return new OrderItemDto
            {
                ProductId = int.TryParse( item.ProductId, out var id ) ? id : -1,
                ProductName = item.ProductName,
                ImageUri = item.ImageUri,
                Price = item.Price,
                NumberOfItems = item.Quantity
            };
        }
    }
}
