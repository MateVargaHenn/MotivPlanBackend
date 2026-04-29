using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Infrastructure.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow.ToLocalTime();

    public DateOnly UtcDateNow => DateOnly.FromDateTime(UtcNow);

    public TimeOnly UtcTimeNow => TimeOnly.FromDateTime(UtcNow);

    public DateTimeOffset UtcDateOffsetNow => DateTime.SpecifyKind(UtcNow, DateTimeKind.Utc);

    public DateTime UtcNowAt(int hour, int minute, int second)
    {
        var now = UtcNow;
        return new DateTime(
            now.Year,
            now.Month,
            now.Day,
            hour, minute, second,
            now.Kind
            );
    }
}
