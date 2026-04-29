using Microsoft.Extensions.Logging;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Shared.Common;
using Serilog.Context;

internal static partial class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;

            DecoratorLogs.ProcessingCommand(logger, commandName);

            Result<TResponse> result = await innerHandler.Handle(request, cancellationToken);

            if (result.IsSuccess)
            {
                DecoratorLogs.CompletedCommand(logger, commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    DecoratorLogs.CompletedCommandWithError(logger, commandName);
                }
            }

            return result;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;

            DecoratorLogs.ProcessingCommand(logger, commandName);

            Result result = await innerHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
            {
                DecoratorLogs.CompletedCommand(logger, commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    DecoratorLogs.CompletedCommandWithError(logger, commandName);
                }
            }

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            string queryName = typeof(TQuery).Name;

            DecoratorLogs.ProcessingQuery(logger, queryName);

            Result<TResponse> result = await innerHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
            {
                DecoratorLogs.CompletedQuery(logger, queryName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    DecoratorLogs.CompletedQueryWithError(logger, queryName);
                }
            }

            return result;
        }
    }
}

internal static partial class DecoratorLogs
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Processing command {Command}")]
    internal static partial void ProcessingCommand(ILogger logger, string command);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Completed command {Command}")]
    internal static partial void CompletedCommand(ILogger logger, string command);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "Completed command {Command} with error")]
    internal static partial void CompletedCommandWithError(ILogger logger, string command);

    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Information,
        Message = "Processing query {Query}")]
    internal static partial void ProcessingQuery(ILogger logger, string query);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Information,
        Message = "Completed query {Query}")]
    internal static partial void CompletedQuery(ILogger logger, string query);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Error,
        Message = "Completed query {Query} with error")]
    internal static partial void CompletedQueryWithError(ILogger logger, string query);
}