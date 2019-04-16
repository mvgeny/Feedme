using Autofac;
using Feedme.Application.Commands;
using Feedme.Application.Queries;
using Feedme.Application.RequestHandling;
using Feedme.Data.ConnectionStrings;
using Feedme.Data.Repositories;
using Feedme.Domain.Interfaces;
using Feedme.Infrastructure.Parser;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Feedme.Api.CompositionRoot
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configuration;
        public AutofacModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FeedRepository>().As<IFeedRepository>();
            builder.RegisterAssemblyTypes(typeof(IQueryHandler<,>).Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterGenericDecorator(typeof(LoggingCommandDecorator<>), typeof(ICommandHandler<>));
            builder.RegisterDecorator<CachedGetFeedNewsQueryHandler, IQueryHandler<GetFeedNewsQuery, News[]>>();

            var queryConnectionString = new QueryConnectionString(_configuration.GetConnectionString("Default"));
            builder.RegisterInstance(queryConnectionString).As<IQueryConnectionString>().SingleInstance();
            builder.RegisterType<RequestHandler>().SingleInstance();
            builder.RegisterType<ParserProcessor>().As<IParserProcessor>().SingleInstance();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
        }
    }
}