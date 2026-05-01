using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;

namespace MotivPlanBackend.Application.Abstractions.Data;

public interface IMotivPlanDbContext
{
    DbSet<WorkoutEntity> Workouts { get; }
    DbSet<ExerciseEntity> Exercises { get; }
    DbSet<WorkoutExerciseEntity> WorkoutsExercises { get; }
    DbSet<WorkoutStatusEntity> WorkoutStatus { get; }
    DbSet<UserWorkoutEntity> UserWorkout { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}