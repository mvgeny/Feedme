using System;
using System.Threading;
using System.Threading.Tasks;

namespace Feedme.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));
    }
}