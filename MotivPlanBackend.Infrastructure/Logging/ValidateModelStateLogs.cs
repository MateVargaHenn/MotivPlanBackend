using Microsoft.Extensions.Logging;

namespace MotivPlanBackend.Infrastructure.Logging;

public static partial class ValidateModelStateLogs
{
    [LoggerMessage(
    EventId = 1001,
    Level = LogLevel.Debug,
    Message = "Validation errors: {ValidationErrors}")]
    public static partial void ValidationErrors(
    ILogger logger,
    string validationErrors);
}
