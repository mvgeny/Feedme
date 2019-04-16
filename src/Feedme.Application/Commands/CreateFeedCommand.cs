using System;
using System.Threading.Tasks;
using Feedme.Application.RequestHandling;
using Feedme.Domain.Functional;
using Feedme.Domain.Interfaces;
using Feedme.Domain.Models;

namespace Feedme.Application.Commands
{
    public class CreateFeedCommand : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }

        public CreateFeedCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public sealed class CreateFeedCommandHandler : ICommandHandler<CreateFeedCommand>
        {
            private readonly IFeedRepository _feedRepository;

            public CreateFeedCommandHandler(IFeedRepository feedRepository)
            {
                _feedRepository = feedRepository ?? throw new ArgumentNullException(nameof(feedRepository));
            }
            public async Task<Result> Handle(CreateFeedCommand command)
            {
                var feedResult = Feed.Create(command.Id, command.Name);
                if (feedResult.IsFailure)
                {
                    return Result.Fail(feedResult.Error);
                }
                await _feedRepository.Add(feedResult.Value);
                await _feedRepository.UnitOfWork.SaveChangesAsync();
                return Result.Ok();
            }
        }
    }
}