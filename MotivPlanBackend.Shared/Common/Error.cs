namespace MotivPlanBackend.Shared.Common;

public record Error
{
    public static readonly Error None = new(null!, ErrorType.None);
    public static readonly Error NullValue = new(
        null!,
        ErrorType.NullValue);

    public Error(string obj, ErrorType type)
    {
        Obj = obj?.ToString() ?? string.Empty;
        Type = type;
    }

    public string Obj { get; }

    public ErrorType Type { get; }

    public static Error InvalidCredentials(string obj) => new(obj,  ErrorType.Unauthorized);

    public static Error Failure(string obj) =>
        new(obj, ErrorType.Failure);

    public static Error NotFound(string obj) =>
        new(obj, ErrorType.NotFound);

    public static Error Problem(string obj) =>
        new(obj, ErrorType.Problem);

    public static Error Conflict(string obj) =>
        new(obj, ErrorType.Conflict);
}
