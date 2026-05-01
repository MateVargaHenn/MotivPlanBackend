using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Domain.Entities;

public sealed class WorkoutEntity : Entity
{
    private readonly List<WorkoutExerciseEntity> _workoutsExercises = [];

    public string Title { get; set; } = string.Empty;
    public IReadOnlyCollection<WorkoutExerciseEntity> WorkoutsExercises =>
        _workoutsExercises.AsReadOnly();

    public ICollection<UserWorkoutEntity> UsersWorkouts { get; } = [];

    public void AddExercise(
        int exerciseId,
        int sets,
        int? reps = null,
        TimeSpan? duration = null)
    {
        _workoutsExercises.Add(new WorkoutExerciseEntity
        {
            ExerciseId = exerciseId,
            Sets = sets,
            Reps = reps,
            Duration = duration,
            IsTime = duration.HasValue
        });
    }
}
