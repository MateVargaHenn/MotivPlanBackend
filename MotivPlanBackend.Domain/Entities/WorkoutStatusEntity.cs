using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Domain.Entities;

public class WorkoutStatusEntity : EnumEntity<WorkoutStatus>
{
    public virtual ICollection<UserWorkoutEntity> UsersWorkouts { get; } = [];
}
