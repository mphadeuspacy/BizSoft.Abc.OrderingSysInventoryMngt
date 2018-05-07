using System;
using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.Entities.OrderItem;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BizSoft.Ordering.EntityFrameworkCore
{
    public class OrderingDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderingDbContext(DbContextOptions<OrderingDbContext> dbContextOptions, IMediator mediator)
            :
            base(dbContextOptions)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            return await SaveChangesAsync(cancellationToken) > default(int);
        }
    }

    //TODO: Uncomment & fix this
    //public class OrderingDbContextDesignFactory : IDesignTimeDbContextFactory<OrderingDbContext>
    //{
    //    public OrderingDbContext CreateDbContext( string[] args )
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<OrderingDbContext>()
    //            .UseSqlServer( "Server=.;Initial Catalog=BizSoft.Services.OrderingDb;Integrated Security=true" );

    //        return new OrderingDbContext( optionsBuilder.Options, new NoMediator() );
    //    }

    //    internal class NoMediator : IMediator
    //    {
    //        public Task Publish<TNotification>( TNotification notification, CancellationToken cancellationToken = default( CancellationToken ) ) where TNotification : INotification
    //        {
    //            return Task.CompletedTask;
    //        }

    //        public Task<TResponse> Send<TResponse>( IRequest<TResponse> request, CancellationToken cancellationToken = default( CancellationToken ) )
    //        {
    //            return Task.FromResult( default( TResponse ) );
    //        }

    //        public Task Send( IRequest request, CancellationToken cancellationToken = default( CancellationToken ) )
    //        {
    //            return Task.CompletedTask;
    //        }
    //    }
    //}
}
