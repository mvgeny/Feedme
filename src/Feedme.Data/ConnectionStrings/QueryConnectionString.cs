using System;
using Feedme.Domain.Interfaces;

namespace Feedme.Data.ConnectionStrings
{
    public class QueryConnectionString : IQueryConnectionString
    {
        public string Value { get; }

        public QueryConnectionString(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}