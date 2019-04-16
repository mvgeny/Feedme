using System.Collections.Generic;
using System.Threading.Tasks;
using Feedme.Domain.Models;

namespace Feedme.Infrastructure.Parser
{
    public interface IParserProcessor
    {
         Task<News[]> GetFeedsNews(List<KeyValuePair<SourceType, string>> list);
    }
}