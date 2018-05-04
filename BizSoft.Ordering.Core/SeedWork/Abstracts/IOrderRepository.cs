using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Create(Order order);
        void Modify(Order order);
        Task<Order> RetrieveByIdAsync(int orderId);
    }
}
