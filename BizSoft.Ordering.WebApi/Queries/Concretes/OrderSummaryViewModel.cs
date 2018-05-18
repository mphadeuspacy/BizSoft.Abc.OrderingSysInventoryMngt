using System;

namespace BizSoft.Ordering.WebApi.Queries.Concretes
{
    public class OrderSummaryViewModel
    {
        public int Ordernumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
    }
}
