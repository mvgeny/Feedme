using System;

namespace Feedme.Infrastructure.Parser
{
    public class Post
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
    }
}