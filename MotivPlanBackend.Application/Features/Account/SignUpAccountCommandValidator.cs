using FluentValidation;

namespace MotivPlanBackend.Application.Features.Account;

public class SignUpAccountCommandValidator : AbstractValidator<SignUpAccountCommand>
{
    public SignUpAccountCommandValidator()
    {
        RuleFor(x => x.SignUpAccountDto)
            .NotNull().WithMessage("Sign Up Account data is required.");
        RuleFor(x => x.SignUpAccountDto.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(6).WithMessage("Username must be at least 6 characters long.")
            .MaximumLength(12).WithMessage("Username must not exceed 12 characters.");
        RuleFor(x => x.SignUpAccountDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.SignUpAccountDto.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters.");
        RuleFor(x => x.SignUpAccountDto.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .MinimumLength(8).WithMessage("Confirm Password must be at least 8 characters long.")
            .MaximumLength(20).WithMessage("Confirm Password must not exceed 20 characters.")
            .Must((command, confirmPassword) => confirmPassword == command.SignUpAccountDto.Password)
            .WithMessage("Passwords do not match.");
        RuleFor(x => x.SignUpAccountDto.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(30).WithMessage("First Name must not exceed 30 characters.");
        RuleFor(x => x.SignUpAccountDto.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(30).WithMessage("Last Name must not exceed 30 characters.");
        RuleFor(x => x.SignUpAccountDto.BirthDate)
            .NotEmpty().WithMessage("Birth Date is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Birth Date must be in the past.");
        RuleFor(x => x.SignUpAccountDto.Sex)
            .NotEmpty().WithMessage("Sex is required.")
            .IsInEnum().WithMessage("Invalid sex value.");
        RuleFor(x => x.SignUpAccountDto.Weight)
            .NotEmpty().WithMessage("Weight is required.")
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.SignUpAccountDto.Height)
            .NotEmpty().WithMessage("Height is required.")
            .GreaterThan(0).WithMessage("Height must be greater than 0.");
        RuleFor(x => x.SignUpAccountDto.Preferences)
            .NotEmpty().WithMessage("Preference is required.")
            .IsInEnum().WithMessage("Invalid preference value.");
    }
}
