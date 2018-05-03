using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Create(Order order);
        void Update(Order order);
        Task<Order> GetByIdAsync(int orderId);
    }
}
