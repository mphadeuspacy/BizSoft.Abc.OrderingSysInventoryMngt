namespace Catalog.WebApi.Dtos
{
    public class OrderStockItemDto
    {
        public int ProductId { get; }
        public int NumberOfUnits { get; }

        public OrderStockItemDto( int productId, int numberOfUnits )
        {
            ProductId = productId;
            NumberOfUnits = numberOfUnits;
        }
    }
}
