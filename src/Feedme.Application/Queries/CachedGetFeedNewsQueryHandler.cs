using System;
using System.Threading.Tasks;
using Feedme.Application.RequestHandling;
using Feedme.Domain.Functional;
using Feedme.Infrastructure.Parser;
using Microsoft.Extensions.Caching.Memory;

namespace Feedme.Application.Queries
{
    public class CachedGetFeedNewsQueryHandler : IQueryHandler<GetFeedNewsQuery, News[]>
    {
        private readonly IQueryHandler<GetFeedNewsQuery, News[]> _decoratee;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private const string CacheKey = "FeedNews";

        public CachedGetFeedNewsQueryHandler(IQueryHandler<GetFeedNewsQuery, News[]> decoratee, IMemoryCache cache)
        {
            _decoratee = decoratee;
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
        }
        public Task<Result<News[]>> Handle(GetFeedNewsQuery query)
        {
            string key = CacheKey + "-" + query.FeedTextGuid;

            return _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _decoratee.Handle(query);
            });
        }
    }
}