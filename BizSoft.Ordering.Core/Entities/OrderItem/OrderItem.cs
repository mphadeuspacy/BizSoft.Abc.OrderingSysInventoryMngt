using BizSoft.Ordering.Core.Exceptions;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.Core.Entities.OrderItem
{
    public class OrderItem : Entity
    {
        private readonly string _productName;
        private readonly string _imageUri;
        private readonly decimal _price;
        private int _numberOfItems;

        public int ProductId { get;}

        public string GetProductName() => _productName;
        public string GetImageUri () => _imageUri;
        public int GetNumberOfItems() => _numberOfItems;
        public decimal GetPrice() => _price;

        protected OrderItem() { }

        public OrderItem
        (
            int productId,
            string productName,
            decimal price,
            string imageUri,
            int numberOfItems
        )
        {
            if (numberOfItems <= 0) throw new OrderingDomainException("Number of order items is not valid");

            ProductId = productId;
            _productName = productName;
            _price = price;
            _imageUri = imageUri;
            _numberOfItems = numberOfItems;
        }

        public void AddOrderItems( int numberOfitems )
        {
            if (numberOfitems < 0)
            {
                throw new OrderingDomainException( "Number of order items is not valid" );
            }

            _numberOfItems += numberOfitems;
        }
    }
}
