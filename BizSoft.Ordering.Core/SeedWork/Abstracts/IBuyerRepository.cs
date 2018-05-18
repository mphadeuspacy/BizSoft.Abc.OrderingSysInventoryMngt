using System.Threading.Tasks;
using BizSoft.Ordering.Core.AggregateEntities.Buyer;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IBuyerRepository : IRepository<Buyer>
    {
        Task<Buyer> FindAsync( string BuyerIdentityGuid );
        Task<Buyer> FindByIdAsync( string id );
    }
}
