using System;
using System.Linq;
using System.Threading.Tasks;
using Feedme.Application.RequestHandling;
using Feedme.Application.Validations;
using Feedme.Domain.Functional;
using Feedme.Domain.Interfaces;
using Feedme.Domain.Models;

namespace Feedme.Application.Commands
{
    public class AddSourceCommand : ICommand
    {
        public string TextFeedGuid { get; }
        public string Url { get; }
        public string TextSourceType { get; set; }

        public AddSourceCommand(string textFeedGuid, string url, string type)
        {
            TextFeedGuid = textFeedGuid;
            Url = url;
            TextSourceType = type;
        }

        public sealed class AddRssCommandHandler : ICommandHandler<AddSourceCommand>
        {
            private readonly IFeedRepository _feedRepository; 

            public AddRssCommandHandler(IFeedRepository feedRepository)
            {
                _feedRepository = feedRepository ?? throw new ArgumentNullException(nameof(feedRepository));
            }
            public async Task<Result> Handle(AddSourceCommand command)
            {
                var feedGuidResult = ValidationHelper.ConvertToGuid(command.TextFeedGuid);
                var sourceTypeResult = ValidationHelper.ConvertToSourceType(command.TextSourceType);
                var validationResult = Result.Combine(feedGuidResult, sourceTypeResult);
                if (validationResult.IsFailure)
                {
                    return Result.Fail(validationResult.Error);
                }
                var feedMaybe = await _feedRepository.Get(feedGuidResult.Value);
                if (feedMaybe.HasNoValue)
                {
                    return Result.Fail($"Feed with id {feedGuidResult.Value} doesn't exist");
                }
                var newSourceResult = Source.Create(command.Url, sourceTypeResult.Value);
                return newSourceResult
                    .OnSuccess(() => feedMaybe.Value.AddSource(newSourceResult.Value))
                    .OnSuccess(async () => await _feedRepository.UnitOfWork.SaveChangesAsync())
                    .OnBoth(x => x.IsSuccess ? Result.Ok() : Result.Fail(x.Error));
            }
        }
    }
}