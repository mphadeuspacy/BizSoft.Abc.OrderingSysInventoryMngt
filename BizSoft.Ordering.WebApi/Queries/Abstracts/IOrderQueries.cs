using System.Collections.Generic;
using System.Threading.Tasks;
using BizSoft.Ordering.WebApi.Queries.Concretes;

namespace BizSoft.Ordering.WebApi.Queries.Abstracts
{
    public interface IOrderQueries
    {
        Task<OrderViewModel> GetOrderAsync( int id );

        Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync();
    }
}
