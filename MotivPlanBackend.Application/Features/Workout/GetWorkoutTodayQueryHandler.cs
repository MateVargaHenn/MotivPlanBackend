using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;
using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.Features.Workout;

public sealed class GetWorkoutTodayQueryHandler(IMotivPlanDbContext context, ILogger<GetWorkoutByIdQueryHandler> logger) : IQueryHandler<GetWorkoutTodayQuery, WorkoutExerciseDataTransferObject>
{
    private readonly IMotivPlanDbContext _context = context;
    private readonly ILogger<GetWorkoutByIdQueryHandler> _logger = logger;
    public async Task<Result<WorkoutExerciseDataTransferObject>> Handle(GetWorkoutTodayQuery request, CancellationToken cancellationToken)
    {
        _logger.StartQueuingWorkout();
        var workout = await _context.UserWorkout
            .Include(userWorkout => userWorkout.Workout)
            .Where(userWorkout => userWorkout.Schedule == DateOnly.FromDateTime(DateTime.UtcNow))
            .Select(userWorkout => new WorkoutExerciseDataTransferObject
            (
                userWorkout.Id,
                userWorkout.Workout.Title,
                userWorkout.Schedule,
                userWorkout.WorkoutStatus,
                userWorkout.Workout.WorkoutsExercises.Select(we => new ExerciseDataTransferObject
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

        _logger.CheckWorkoutInstanceIsNotNull();
        if (workout == null)
        {
            _logger.WorkoutQueryFailedToday();
            return Result.Failure<WorkoutExerciseDataTransferObject>(Error.NotFound(nameof(workout)));
        }
        _logger.WorkoutIsNotNullReturningInstance(workout.Id);
        return Result<WorkoutExerciseDataTransferObject>.Success(workout);
    }
}
