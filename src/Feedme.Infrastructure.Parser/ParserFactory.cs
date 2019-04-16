using System;
using System.Collections.Generic;
using Feedme.Domain.Models;
using Feedme.Infrastructure.Parser.Parsers;

namespace Feedme.Infrastructure.Parser
{
    internal static class ParserFactory
    {
        private static readonly Dictionary<SourceType, Func<IParser>> _map =
            new Dictionary<SourceType, Func<IParser>>();

        static ParserFactory()
        {
            _map[SourceType.Rss] = () => new RssParser();
            _map[SourceType.Atom] = () => new AtomParser();
        }

        public static IParser Create(SourceType type)
        {
            var creator = GetCreator(type);
            if (creator == null)
                throw new NotSupportedException(nameof(type));
            return creator();
        }

        private static Func<IParser> GetCreator(SourceType type)
        {
            _map.TryGetValue(type, out var creator);
            return creator;
        }
    }
}