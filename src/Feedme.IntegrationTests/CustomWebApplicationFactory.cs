using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Feedme.Api;
using Feedme.Data.ConnectionStrings;
using Feedme.Data.Context;
using Feedme.Domain.Interfaces;
using Feedme.Domain.Models;
using Feedme.Infrastructure.Parser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feedme.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var testConnectionString = configuration.GetConnectionString("Test");
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlServer()
                    .BuildServiceProvider();
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(testConnectionString);
                    options.UseInternalServiceProvider(serviceProvider);
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }
            });
        }
    }
}