using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feedme.Domain.Models;

namespace Feedme.Infrastructure.Parser
{
    public class ParserProcessor : IParserProcessor
    {
        public async Task<News[]> GetFeedsNews(List<KeyValuePair<SourceType, string>> list)
        {
            var tasks = list.Select(x => LoadNews(x.Key, x.Value));
            return await Task.WhenAll(tasks);
        }

        private Task<News> LoadNews(SourceType type, string url)
        {
            return Task.Run(() =>
             {
                 var parser = ParserFactory.Create(type);
                 var postsResult = parser.Parse(url);
                 if (postsResult.IsSuccess)
                 {
                     return new News
                     {
                         Success = true,
                         Url = url,
                         Type = type,
                         Posts = postsResult.Value
                     };
                 }

                 return new News
                 {
                     Success = false,
                     Url = url,
                     Type = type,
                 };
             });
        }
    }
}