namespace MotivPlanBackend.Shared.Common;

public class Result
{
    private readonly Error? _error;
    private readonly ValidationError? _validationError;
    public Result(bool isSuccess, Error error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "Error cannot be null.");
        }
        if (isSuccess && error.Type != ErrorType.None)
        {
            throw new ArgumentException("Successful result cannot have an error.", nameof(error));
        }

        if (!isSuccess && (error.Type == ErrorType.None))
        {
            throw new ArgumentException("Failed result must have a valid error.", nameof(error));
        }

        IsSuccess = isSuccess;
        _error = error;
    }
    public Result(bool isSuccess, ValidationError error)
    {
        if (isSuccess && error != null ||
            !isSuccess && error == null)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        _validationError = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ValidationError? ValidationError => _validationError;

    public Error Error => IsSuccess
        ? throw new InvalidOperationException("The error of a success result can't be accessed.")
        : _error!;

    public static Result Success() => new(true, new Error(null!, ErrorType.None));

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, new Error(null!, ErrorType.None));

    public static Result Failure(ErrorType error) => new(false, new Error(error.ToString(), error));

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);

    public static Result Failure(ValidationError error) =>
        new(false, error);
}

public class Result<TValue> : Result
{
    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) => Value = value;

    
    public TValue? Value => IsSuccess
        ? field!
        : default;

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(new Error(null!, ErrorType.NullValue));

    public Result<TValue> ValidationFailure(string obj, ErrorType error) =>
        new(default, false, new Error(obj, error));
}