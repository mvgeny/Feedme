using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Feedme.Domain.Functional;

namespace Feedme.Infrastructure.Parser.Parsers
{
    internal class RssParser : IParser
    {
        public Result<List<Post>> Parse(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                DateTime publishDate;
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                            select new Post
                            {
                                Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                PublishDate = DateTime.TryParse(item.Elements().First(i => i.Name.LocalName == "pubDate").Value, out publishDate) ? publishDate : DateTime.MinValue,
                                Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                            };
                return Result.Ok(entries.ToList());
            }
            catch
            {
                return Result.Fail<List<Post>>($"Error while loading {url} data");
            }
        }
    }
}