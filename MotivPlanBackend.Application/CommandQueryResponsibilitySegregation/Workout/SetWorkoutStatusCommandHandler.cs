using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;

public sealed class SetWorkoutStatusCommandHandler(IMotivPlanDbContext context) : ICommandHandler<SetWorkoutStatusCommand>
{
    private readonly IMotivPlanDbContext _context = context;
    public async Task<Result> Handle(SetWorkoutStatusCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return Result.Failure(ErrorType.NullValue);
        }

        var userWorkout = await _context.UserWorkout.Where(x => x.WorkoutId == request.WorkoutStatusDto.WorkoutId).FirstOrDefaultAsync(cancellationToken);
        if (userWorkout == null)
        {
            return Result.Failure(ErrorType.NotFound);
        }
        userWorkout.WorkoutStatus = request.WorkoutStatusDto.Status;
        switch (request.WorkoutStatusDto.Status)
        {
            case WorkoutStatus.InProgress:
                userWorkout.StartedAt = DateTime.UtcNow;
                break;
            case WorkoutStatus.Completed:
                userWorkout.CompletedAt = DateTime.UtcNow;
                break;
            case WorkoutStatus.Cancelled:
                userWorkout.CancelledAt = DateTime.UtcNow;
                break;
            default:
                break;
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
