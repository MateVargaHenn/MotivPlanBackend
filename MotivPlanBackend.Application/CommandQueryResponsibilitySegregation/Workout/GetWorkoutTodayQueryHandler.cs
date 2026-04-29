using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;

public sealed class GetWorkoutTodayQueryHandler(IMotivPlanDbContext context, ILogger<GetWorkoutByIdQueryHandler> logger) : IQueryHandler<GetWorkoutTodayQuery, WorkoutExerciseDataTransferObject>
{
    private readonly IMotivPlanDbContext _context = context;
    private readonly ILogger<GetWorkoutByIdQueryHandler> _logger = logger;
    public async Task<Result<WorkoutExerciseDataTransferObject>> Handle(GetWorkoutTodayQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start queueing Workout...");
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

        _logger.LogInformation("Check workout instance is not null...");
        if (workout == null)
        {
            _logger.LogError("Workout instance is null. Return not found status.");
            return Result.Failure<WorkoutExerciseDataTransferObject>(Error.NotFound(nameof(workout)));
        }

        _logger.LogInformation("Workout is not null. Return instance.", workout);
        return Result<WorkoutExerciseDataTransferObject>.Success(workout);
    }
}
