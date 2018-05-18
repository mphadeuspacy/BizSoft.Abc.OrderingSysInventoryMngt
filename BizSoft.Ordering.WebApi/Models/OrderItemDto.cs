namespace BizSoft.Ordering.WebApi.Models
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int NumberOfItems { get; set; }
        public string ImageUri { get; set; }
    }
}
