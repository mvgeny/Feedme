using System.Threading.Tasks;
using Feedme.Domain.Functional;

namespace Feedme.Application.RequestHandling
{
    public class ICommand
    {
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command);
    }
}