using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal sealed class WorkoutExerciseEntityModelBuilder : EntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
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
