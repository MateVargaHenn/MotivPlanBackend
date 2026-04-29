namespace MotivPlanBackend.Shared.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateOnly UtcDateNow { get; }
    TimeOnly UtcTimeNow { get; }
    DateTimeOffset UtcDateOffsetNow { get; }

    DateTime UtcNowAt(int hour, int minute, int second);
}
