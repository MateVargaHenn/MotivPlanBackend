using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;

public sealed class GetWorkoutTodayQueryHandler(IMotivPlanDbContext context) : IQueryHandler<GetWorkoutTodayQuery, WorkoutExerciseDataTransferObject>
{
    private readonly IMotivPlanDbContext _context = context;
    public async Task<Result<WorkoutExerciseDataTransferObject>> Handle(GetWorkoutTodayQuery request, CancellationToken cancellationToken)
    {
        var workout = await _context.Workouts
            .Where(w => w.Schedule == DateOnly.FromDateTime(DateTime.UtcNow))
            .Select(w => new WorkoutExerciseDataTransferObject
            (
                w.Id,
                w.Title,
                w.Schedule,
                w.WorkoutsExercises.Select(we => new ExerciseDataTransferObject
                (
                    we.Exercise.Id,
                    we.Exercise.Title,
                    we.Exercise.Description,
                    we.Sets,
                    we.IsTime,
                    we.Reps,
                    we.Duration
                )).ToList()
            )).FirstOrDefaultAsync(cancellationToken);

        if (workout == null)
        {
            return Result.Failure<WorkoutExerciseDataTransferObject>(Error.NotFound(nameof(workout)));
        }

        return Result<WorkoutExerciseDataTransferObject>.Success(workout);
    }
}
