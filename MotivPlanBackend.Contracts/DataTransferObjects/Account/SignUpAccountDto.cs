using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Contracts.DataTransferObjects.Account;

public record SignUpAccountDto(
    string Username,
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    Sex Sex,
    double Weight,
    int Height,
    ICollection<Preference> Preferences);