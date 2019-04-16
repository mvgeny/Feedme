using System;
using System.Threading.Tasks;
using Feedme.Domain.Functional;

namespace Feedme.Application.RequestHandling
{
    public sealed class RequestHandler
    {
        private readonly IServiceProvider _provider;

        public RequestHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<Result> Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Result result = await handler.Handle((dynamic)command);

            return result;
        }

        public async Task<Result<T>> Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Result<T> result = await handler.Handle((dynamic)query);

            return result;
        }
    }
}