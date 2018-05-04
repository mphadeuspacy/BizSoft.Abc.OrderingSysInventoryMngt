using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.EntityFrameworkCore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingDbContext _context;
        public IDbContextPersister DbContextPersister { get; }
        public Order Create(Order order)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(Order order)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> RetrieveByIdAsync(int orderId)
        {
            throw new System.NotImplementedException();
        }
    }
}
