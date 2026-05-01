namespace MotivPlanBackend.Shared.Common;

public sealed record ValidationError : Error
{
    public ValidationError(IReadOnlyList<Error> errors)
        : base("General.Error", ErrorType.Failure) => Errors = errors;

    public IReadOnlyList<Error> Errors { get; } = [];
}
