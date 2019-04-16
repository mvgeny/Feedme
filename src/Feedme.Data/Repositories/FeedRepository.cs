using System;
using System.Threading.Tasks;
using Feedme.Data.Context;
using Feedme.Domain.Functional;
using Feedme.Domain.Common;
using Feedme.Domain.Interfaces;
using Feedme.Domain.Models;

namespace Feedme.Data.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public FeedRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Feed feed)
        {
            await _context.Feeds.AddAsync(feed);
        }

        public async Task<Maybe<Feed>> Get(Guid feedId)
        {
            var feed =  await _context.Feeds.FindAsync(feedId);
            if (feed == null)
            {
                return new Maybe<Feed>();
            }
            await _context.Entry(feed).Collection(x => x.RssColection).LoadAsync();
            return new Maybe<Feed>(feed);
        }
    }
}