using System;
using System.Threading.Tasks;
using Feedme.Domain.Common;
using Feedme.Domain.Functional;
using Feedme.Domain.Models;

namespace Feedme.Domain.Interfaces
{
    public interface IFeedRepository : IRepository<Feed>
    {
        Task Add (Feed feed);
        Task<Maybe<Feed>> Get(Guid feedId);
    }
}