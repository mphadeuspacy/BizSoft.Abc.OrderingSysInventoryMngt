using System;
using System.Threading;
using System.Threading.Tasks;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
