using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace BizSoft.Ordering.EntityFrameworkCore
{
    public class OrderingDbContext : DbContext, IDbContextPersister
    {
        public Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}
