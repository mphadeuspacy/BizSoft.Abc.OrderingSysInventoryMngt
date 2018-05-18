using System;
using System.Linq;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.AggregateEntities.Buyer;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace BizSoft.Ordering.EntityFrameworkCore.Repositories
{
    public class BuyerRepository
        : IBuyerRepository
    {
        private readonly OrderingDbContext _orderingDbcontext;
        public IUnitOfWork UnitOfWork => _orderingDbcontext;

        public BuyerRepository( OrderingDbContext context )
        {
            _orderingDbcontext = context ?? throw new ArgumentNullException( nameof( context ) );
        }

        
        public async Task<Buyer> FindAsync( string identity )
        {
            var buyer = await _orderingDbcontext.Buyers
                .Where( b => b.IdentityGuid == identity )
                .SingleOrDefaultAsync();

            return buyer;
        }

        public async Task<Buyer> FindByIdAsync( string id )
        {
            var buyer = await _orderingDbcontext.Buyers
                .Where( b => b.Id == int.Parse( id ) )
                .SingleOrDefaultAsync();

            return buyer;
        }
    }
}
