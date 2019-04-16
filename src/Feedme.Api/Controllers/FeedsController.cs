using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Feedme.Api.Dtos;
using Feedme.Application.Commands;
using Feedme.Application.RequestHandling;
using Feedme.Domain;
using Feedme.Application.Queries;

namespace Feedme.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedsController : ApiController
    {
        public FeedsController(RequestHandler handler) : base(handler) {}

        [HttpPost]
        public async Task<IActionResult> CreateFeed([FromBody] CreateFeedDto dto)
        {
            var guid = Guid.NewGuid();
            var createResult = await _handler.Dispatch(new CreateFeedCommand(guid, dto.Name));
            return createResult.IsSuccess ? Ok(guid) : Error(createResult.Error);
        }

        [HttpPost("{feedId}/source")]
        public async Task<IActionResult> AddSource(string feedId, [FromBody] AddSourceDto dto)
        {
            var result = await _handler.Dispatch(new AddSourceCommand(feedId, dto.Url, dto.Type));
            return FromResult(result);
        }

        [HttpGet("{feedId}/news")]
        public async Task<IActionResult> GetFeedNews(string feedId)
        {
            var result = await _handler.Dispatch(new GetFeedNewsQuery(feedId));
            return FromResult(result);
        }
    }
}
