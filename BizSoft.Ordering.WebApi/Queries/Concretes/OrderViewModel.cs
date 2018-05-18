using System;
using System.Collections.Generic;

namespace BizSoft.Ordering.WebApi.Queries.Concretes
{
    public class OrderViewModel
    {
        public int Ordernumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal Total { get; set; }
    }
}
