using System;
using System.Net.Http;
using Feedme.Api;
using Feedme.Data.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Feedme.IntegrationTests
{
    public class DbContextFixture : IDisposable
    {
        private IDbContextTransaction transaction;
        protected AppDbContext DbContext { get; }
        protected readonly HttpClient Client;

        public DbContextFixture(WebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
            DbContext = factory.Server.Host.Services.GetService<AppDbContext>();
            transaction = DbContext.Database.BeginTransaction();
        }
        
        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }
    }
}