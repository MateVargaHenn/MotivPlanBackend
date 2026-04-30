using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;

public sealed class GetWorkoutByIdQueryHandler(IMotivPlanDbContext context) : IQueryHandler<GetWorkoutByIdQuery, WorkoutExerciseDataTransferObject>
{
    private readonly IMotivPlanDbContext _context = context;
    public async Task<Result<WorkoutExerciseDataTransferObject>> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        var workout = await _context.UserWorkout
            .Include(x => x.Workout)
            .ThenInclude(w => w.WorkoutsExercises)
            .Where(w => w.Id == request.WorkoutDto.Id)
            .Select(w => new WorkoutExerciseDataTransferObject
            (
                w.Id,
                w.Workout.Title,
                w.Schedule,
                w.WorkoutStatus,
                w.Workout.WorkoutsExercises.Select(we => new ExerciseDataTransferObject
                (
                    we.Exercise.Id,
                    we.Exercise.Title,
                    we.Exercise.Description,
                    we.Sets,
                    we.IsTime,
                    we.Reps,
                    we.Duration
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (workout == null)
        {
            return Result.Failure<WorkoutExerciseDataTransferObject>(Error.NotFound(nameof(workout)));
        }

        return Result<WorkoutExerciseDataTransferObject>.Success(workout);
    }
}
