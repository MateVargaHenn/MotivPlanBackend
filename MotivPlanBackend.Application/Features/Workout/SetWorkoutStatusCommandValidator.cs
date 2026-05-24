using FluentValidation;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

namespace MotivPlanBackend.Application.Features.Workout;

public sealed class SetWorkoutStatusCommandValidator : AbstractValidator<SetWorkoutStatusCommand>
{
    public SetWorkoutStatusCommandValidator()
    {
        RuleFor(x => x.WorkoutStatusDto.WorkoutId)
            .GreaterThan(0).WithMessage("Workout ID must be greater than 0.");
        RuleFor(x => x.WorkoutStatusDto.Status)
            .NotEmpty().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Invalid status value.");
    }
}
