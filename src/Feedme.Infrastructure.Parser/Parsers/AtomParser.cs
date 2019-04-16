using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Feedme.Domain.Functional;

namespace Feedme.Infrastructure.Parser.Parsers
{
    internal class AtomParser : IParser
    {
        public Result<List<Post>> Parse(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                DateTime publishDate;
                var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                            select new Post
                            {
                                Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
                                Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                                PublishDate = DateTime.TryParse(item.Elements().First(i => i.Name.LocalName == "published").Value, out publishDate)
                                    ? publishDate
                                    : DateTime.MaxValue,
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