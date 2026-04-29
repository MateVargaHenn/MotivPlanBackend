using System.Runtime.CompilerServices;

namespace MotivPlanBackend.Shared.Common;

public static class Ensure
{
    public static void NotNull(
    object? value,
    [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == null)
        {
            throw new UnauthorizedAccessException($"Unauthorized access: {paramName}");
        }
    }
}
