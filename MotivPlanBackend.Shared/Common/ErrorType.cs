namespace MotivPlanBackend.Shared.Common;

public enum ErrorType
{
    None = 0,
    Validation = 1,
    Unauthorized = 2,
    NotFound = 3,
    Forbidden = 4,
    Conflict = 5,
    NullValue = 6,
    Failure = 7,
    Problem = 8,
    Unknown = 9
}
