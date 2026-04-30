using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotivPlanBackend.Domain.Entities;

public sealed class UserWorkoutEntity : Entity
{
    public int UserId { get; set; }

    [ForeignKey(nameof(WorkoutEntity))]
    public int WorkoutId { get; set; }
    public WorkoutEntity Workout { get; set; } = default!;

    public WorkoutStatus WorkoutStatus { get; set; }
    public WorkoutStatusEntity WorkoutStatusEntity { get; internal set; } = default!;
    public DateOnly Schedule { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}
