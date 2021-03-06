﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.AggregateEntities.Buyer;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.Entities.OrderItem;
using BizSoft.Ordering.Core.Entities.OrderStatus;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore.Configurations;
using BizSoft.Ordering.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BizSoft.Ordering.EntityFrameworkCore
{
    public class OrderingDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "ordering";

        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        public OrderingDbContext(DbContextOptions<OrderingDbContext> dbContextOptions, IMediator mediator)
            : base(dbContextOptions)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration( new OrderEntityTypeConfiguration() );

            modelBuilder.ApplyConfiguration( new OrderItemEntityTypeConfiguration() );

            modelBuilder.ApplyConfiguration( new OrderStatusEntityTypeConfiguration() );

            modelBuilder.ApplyConfiguration( new BuyerEntityTypeConfiguration() );
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            return await SaveChangesAsync(cancellationToken) > default(int);
        }
    }
}
