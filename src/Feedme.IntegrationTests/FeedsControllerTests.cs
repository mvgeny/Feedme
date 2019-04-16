using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Feedme.Api;
using Feedme.Api.Dtos;
using Feedme.Api.Utils;
using Feedme.Data.Context;
using Feedme.Domain.Models;
using Feedme.Infrastructure.Parser;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Feedme.IntegrationTests
{
    public class FeedsControllerTests : DbContextFixture, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public FeedsControllerTests(CustomWebApplicationFactory<Startup> factory) : base (factory)
        {
        }

        [Fact]
        public async Task CreateFeed_ValidDto_IsPersisted()
        {
            var httpResponse = await Client.PostAsync("/api/feeds",
                new StringContent(JsonConvert.SerializeObject(new CreateFeedDto {Name = "validName"}), Encoding.UTF8, "application/json"));
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var envelope = JsonConvert.DeserializeObject<Envelope<Guid>>(stringResponse);
            var persistedId = (await DbContext.Feeds.SingleAsync()).Id;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            envelope.Result.Should().Be(persistedId);
        }

        [Fact]
        public async Task CreateFeed_InvalidName_ShouldReturnError()
        {
            var inValidName = string.Empty;
            var httpResponse = await Client.PostAsync("/api/feeds",
                new StringContent(JsonConvert.SerializeObject(new CreateFeedDto {Name = inValidName}), Encoding.UTF8, "application/json"));
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var envelope = JsonConvert.DeserializeObject<Envelope<string>>(stringResponse);
            var persistedFeed = await DbContext.Feeds.SingleOrDefaultAsync();
            persistedFeed.Should().BeNull();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            envelope.ErrorMessage.Should().NotBeNullOrEmpty();
        }
    }
}