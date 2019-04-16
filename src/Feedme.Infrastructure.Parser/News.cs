using System.Collections.Generic;
using Feedme.Domain.Models;

namespace Feedme.Infrastructure.Parser
{
    public class News
    {
        public string Url { get; set; }
        public SourceType Type { get; set; }
        public bool Success { get; set; }
        public List<Post> Posts { get; set; }
    }
}