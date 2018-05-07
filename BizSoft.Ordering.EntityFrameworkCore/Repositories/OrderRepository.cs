using System;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.EntityFrameworkCore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingDbContext _orderingDbContext;
        public IUnitOfWork UnitOfWork => _orderingDbContext;

        public OrderRepository(OrderingDbContext orderingDbContext)
        {
            _orderingDbContext = orderingDbContext ?? throw new ArgumentNullException(nameof(orderingDbContext));
        }

        public Order Create(Order order)
        {
            return _orderingDbContext.Orders.Add(order).Entity;
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
