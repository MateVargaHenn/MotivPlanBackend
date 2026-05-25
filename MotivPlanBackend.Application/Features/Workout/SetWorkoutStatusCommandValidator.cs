using FluentValidation;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

namespace MotivPlanBackend.Application.Features.Workout;

public sealed class SetWorkoutStatusCommandValidator : AbstractValidator<SetWorkoutStatusCommand>
{
    private readonly IMotivPlanDbContext _context;
    public SetWorkoutStatusCommandValidator(IMotivPlanDbContext context)
    {
        _context = context;

        RuleFor(x => x.WorkoutStatusDto)
            .NotNull().WithMessage("Workout status is required.");

        When(x => x.WorkoutStatusDto is not null, () =>
        {
            RuleFor(x => x.WorkoutStatusDto.WorkoutId)
            .GreaterThan(-1).WithMessage("Workout ID must be greater than -1.")
            .MustAsync(async (workoutId, cancellationToken) =>
            {
                var workout = await _context.Workouts.FindAsync(new object[] { workoutId }, cancellationToken);
                return workout != null;
            }).WithMessage("Workout not found.");

            RuleFor(x => x.WorkoutStatusDto.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        });
    }
}
