using Microsoft.EntityFrameworkCore;
using Feedme.Data.EntityConfigurations;
using Feedme.Domain.Common;
using Feedme.Domain.Models;

namespace Feedme.Data.Context
{
    public class AppDbContext: DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Source> Sources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SourceMap());
            modelBuilder.ApplyConfiguration(new FeedMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}