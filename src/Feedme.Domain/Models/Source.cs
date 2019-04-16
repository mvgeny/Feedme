using System;
using Feedme.Domain.Common;
using Feedme.Domain.Functional;

namespace Feedme.Domain.Models
{
    public class Source : Entity
    {
        public string Url { get; private set; }
        public SourceType Type { get; private set; }

        protected Source(string url, SourceType type)
        {
            Url = url;
            Type = type;
        }

        public static Result<Source> Create(string url, SourceType type)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var formattedUri)
                && (formattedUri.Scheme == Uri.UriSchemeHttp || formattedUri.Scheme == Uri.UriSchemeHttps))
            {
                return Result.Ok(new Source(formattedUri.AbsoluteUri, type));
            }
            return Result.Fail<Source>("Url of Rss is invalid");
        }
    }
}