using System;
using System.Collections.Generic;
using System.Linq;
using Feedme.Domain.Common;
using Feedme.Domain.Functional;

namespace Feedme.Domain.Models
{
    public class Feed : Entity
    {
        protected Feed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Name  { get; private set; }
        private readonly IList<Source> _rssCollection = new List<Source>();
        public IReadOnlyList<Source> RssColection => _rssCollection.ToList();

        public void AddSource(Source source)
        {
            _rssCollection.Add(source);
        }

        public static Result<Feed> Create(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Fail<Feed>("Name of Feed should not be empty");
            }
            var trimmedName = name.Trim();
            
            if (trimmedName.Length > 100) 
            {
                return Result.Fail<Feed>("Length of name should be less than 100");
            }
            return Result.Ok(new Feed(id, name));
        }
    }
}
