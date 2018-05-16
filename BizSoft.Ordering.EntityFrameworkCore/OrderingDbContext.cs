using System;
using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.Entities.OrderItem;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore.Configurations;
using BizSoft.Ordering.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BizSoft.Ordering.EntityFrameworkCore
{
    public class OrderingDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "ordering";

        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderingDbContext(DbContextOptions<OrderingDbContext> dbContextOptions, IMediator mediator)
            : base(dbContextOptions)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration( new OrderEntityTypeConfiguration() );
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            return await SaveChangesAsync(cancellationToken) > default(int);
        }
    }
}
