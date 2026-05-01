using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Domain.Entities;

public sealed class ExerciseEntity : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<WorkoutExerciseEntity> WorkoutsExercises { get; } = [];
}
