using System;
using System.Threading;
using System.Threading.Tasks;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IDbContextPersister : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
