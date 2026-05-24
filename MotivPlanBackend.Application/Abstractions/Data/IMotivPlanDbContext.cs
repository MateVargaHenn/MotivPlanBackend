using Microsoft.AspNetCore.Identity;
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

    // Identity
    DbSet<IdentityUser> Users { get; }
    DbSet<IdentityRole> Roles { get; }

    // Profile
    DbSet<ProfileEntity> Profiles { get; }
    DbSet<PreferenceEntity> Preferences { get; }
    DbSet<SexEntity> Sex { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}