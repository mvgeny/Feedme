using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Feedme.Application.RequestHandling;
using Feedme.Application.Validations;
using Feedme.Domain.Functional;
using Feedme.Domain.Interfaces;
using Feedme.Domain.Models;
using Feedme.Infrastructure.Parser;

namespace Feedme.Application.Queries
{
    public class GetFeedNewsQuery : IQuery<News[]>
    {
        public string FeedTextGuid { get; set; }

        public GetFeedNewsQuery(string feedTextGuid)
        {
            FeedTextGuid = feedTextGuid;
        }
        public sealed class GetFeedNewsQueryHandler : IQueryHandler<GetFeedNewsQuery, News[]>
        {
            private sealed class SourceDto
            {
                public string Url { get; set; }
                public SourceType Type { get; set; }
            }
            private readonly IQueryConnectionString _queryConnectionString;
            private readonly IParserProcessor _parserProcessor; 

            public GetFeedNewsQueryHandler(IQueryConnectionString queryConnectionString, IParserProcessor parserProcessor)
            {
                _queryConnectionString = queryConnectionString 
                    ?? throw new ArgumentNullException(nameof(queryConnectionString));
                _parserProcessor = parserProcessor 
                    ?? throw new ArgumentNullException(nameof(parserProcessor));
            }
            public async Task<Result<News[]>> Handle(GetFeedNewsQuery query)
            {
                var feedGuidResult = ValidationHelper.ConvertToGuid(query.FeedTextGuid);
                if (feedGuidResult.IsFailure)
                {
                    return Result.Fail<News[]>(feedGuidResult.Error);
                }

                const string sqlQuery = @"
                    SELECT s.Url, s.Type
                    FROM dbo.Sources s
                    WHERE s.FeedId = @FeedId";
                using (var connection = new SqlConnection(_queryConnectionString.Value))
                {
                    var queryResult = await connection
                        .QueryAsync<SourceDto>(sqlQuery, new { FeedId = feedGuidResult.Value});
                    var sources= queryResult.Select(x => new KeyValuePair<SourceType, string>(x.Type, x.Url)).ToList();
                    return Result.Ok(await _parserProcessor.GetFeedsNews(sources));
                }
            }
        }
    }
}