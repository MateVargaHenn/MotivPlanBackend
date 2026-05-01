using Microsoft.Extensions.Logging;

namespace MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;

public static partial class WorkoutLogMessages
{

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Start queuing workout...")]
    public static partial void StartQueuingWorkout(
    this ILogger logger);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = "Check workout instance is not null...")]
    public static partial void CheckWorkoutInstanceIsNotNull(
        this ILogger logger);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "Workout query failed for today")]
    public static partial void WorkoutQueryFailedToday(
        this ILogger logger);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Information,
        Message = "Workout is not null. Returning instance with id {id}.")]
    public static partial void WorkoutIsNotNullReturningInstance(
    this ILogger logger, int id);
}
