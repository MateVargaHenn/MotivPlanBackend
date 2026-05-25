using FluentValidation;

namespace MotivPlanBackend.Application.Features.Workout;

public sealed class GetWorkoutByIdQueryValidator : AbstractValidator<GetWorkoutByIdQuery>
{
    public GetWorkoutByIdQueryValidator()
    {
        RuleFor(x => x.WorkoutDto)
            .NotNull().WithMessage("Workout data transfer object is required.")
            .When(x => x.WorkoutDto is not null)
            .Must(dto => dto.Id > 0).WithMessage("Workout ID must be greater than 0.");
    }
}
