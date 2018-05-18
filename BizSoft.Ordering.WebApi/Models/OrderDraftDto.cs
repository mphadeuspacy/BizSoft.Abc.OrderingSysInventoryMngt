using System.Collections.Generic;
using System.Linq;
using BizSoft.Ordering.Core.Entities.Order;

namespace BizSoft.Ordering.WebApi.Models
{
    public class OrderDraftDto
    {
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
        public decimal Total { get; set; }

        public static OrderDraftDto FromOrder( Order order )
        {
            return new OrderDraftDto
            {
                OrderItems = order.OrderItems.Select( oi => new OrderItemDto
                {
                   ProductId = oi.ProductId,
                    Price = oi.GetPrice(),
                    ImageUri = oi.GetImageUri(),
                    NumberOfItems = oi.GetNumberOfItems(),
                    ProductName = oi.GetProductName()
                } ),

                Total = order.GetTotal()
            };
        }
    }
}
