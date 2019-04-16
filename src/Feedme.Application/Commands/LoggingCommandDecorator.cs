using System;
using System.Threading.Tasks;
using Feedme.Application.RequestHandling;
using Feedme.Domain.Functional;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Feedme.Application.Commands
{
    public sealed class LoggingCommandDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ILogger<TCommand> _logger;
        private readonly ICommandHandler<TCommand> _decoratee;

        public LoggingCommandDecorator(ICommandHandler<TCommand> decoratee, ILogger<TCommand> logger)
        {
            _logger = logger;
            _decoratee = decoratee;
        }
        public async Task<Result> Handle(TCommand command)
        {
            var commandText = JsonConvert.SerializeObject(command);
            _logger.LogInformation($"Command with params {command.GetType().Name}: {commandText} has been executed");
            return await _decoratee.Handle(command);
        }
    }
}