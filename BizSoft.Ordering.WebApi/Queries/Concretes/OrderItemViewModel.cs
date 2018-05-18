namespace BizSoft.Ordering.WebApi.Queries.Concretes
{
    public class OrderItemViewModel
    {
        public string Productname { get; set; }
        public int NumberOfUnits { get; set; }
        public double Price { get; set; }
        public string ImageUri { get; set; }
    }
}
