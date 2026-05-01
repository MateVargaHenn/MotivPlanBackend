namespace MotivPlanBackend.Domain.Entities;

public sealed class WorkoutExerciseEntity 
{
    public int WorkoutId { get; set; }
    public int ExerciseId { get; set; }

    public WorkoutEntity Workout { get; set; } = null!;
    public ExerciseEntity Exercise { get; set; } = null!;

    public bool IsTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public int? Reps { get; set; }
    public int Sets { get; set; }
}