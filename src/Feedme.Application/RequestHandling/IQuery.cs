using System.Threading.Tasks;
using Feedme.Domain.Functional;

namespace Feedme.Application.RequestHandling
{
    public class IQuery<TResult>
    {
    }

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<Result<TResult>> Handle(TQuery query);
    }
}