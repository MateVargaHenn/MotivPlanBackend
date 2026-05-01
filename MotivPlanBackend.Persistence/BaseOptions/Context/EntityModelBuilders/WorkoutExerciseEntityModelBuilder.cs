using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal sealed class WorkoutExerciseEntityModelBuilder : EntityModelBuilderBase
{
#pragma warning disable S2325 // Mark members as static
#pragma warning disable CA1822 // Mark members as static
    public void ConfigureModel(ModelBuilder modelBuilder)
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S2325 // Mark members as static
    {
        modelBuilder.Entity<WorkoutExerciseEntity>(builder =>
        {
            builder
                .HasKey(x => new { x.WorkoutId, x.ExerciseId });

            builder
                .HasOne(x => x.Workout)
                .WithMany(x => x.WorkoutsExercises)
                .HasForeignKey(x => x.WorkoutId);

            builder
                .HasOne(x => x.Exercise)
                .WithMany(x => x.WorkoutsExercises)
                .HasForeignKey(x => x.ExerciseId);

            builder.ToTable("WorkoutExercises", "public");
        });
    }
}
