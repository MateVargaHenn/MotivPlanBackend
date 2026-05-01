using System.ComponentModel;
using System.Reflection;

namespace MotivPlanBackend.Persistence.Extensions;

public static class DescriptionExtension
{
    public static string GetDescription(this Enum value)
    {
        var field = value?.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value?.ToString() ?? string.Empty;
    }
}
