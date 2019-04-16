using System.Collections.Generic;
using Feedme.Domain.Functional;

namespace Feedme.Infrastructure.Parser
{
    public interface IParser
    {
         Result<List<Post>> Parse (string url);
    }
}