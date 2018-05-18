using System;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Exceptions;

namespace BizSoft.Ordering.EntityFrameworkCore.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly OrderingDbContext _orderingDbContext;

        public RequestManager( OrderingDbContext context )
        {
            _orderingDbContext = context ?? throw new ArgumentNullException( nameof( context ) );
        }

        public async Task<bool> ExistAsync( Guid id )
        {
            var request = await _orderingDbContext.FindAsync<ClientRequest>( id );

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>( Guid id )
        {
            var exists = await ExistAsync( id );

            var request = exists ?
                throw new OrderingDomainException( $"Request with {id} already exists" ) :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof( T ).Name,
                    Time = DateTime.UtcNow
                };

            _orderingDbContext.Add( request );

            await _orderingDbContext.SaveChangesAsync();
        }
    }

    public interface IRequestManager
    {
        Task<bool> ExistAsync( Guid id );

        Task CreateRequestForCommandAsync<T>( Guid id );
    }
}