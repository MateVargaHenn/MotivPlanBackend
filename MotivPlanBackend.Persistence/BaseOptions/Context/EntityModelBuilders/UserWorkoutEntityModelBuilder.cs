using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal sealed class UserWorkoutEntityModelBuilder : EntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserWorkoutEntity>(builder =>
        {
            ConfigureCommonProperties(builder);
            builder.HasOne(x => x.Workout).WithMany(x => x.UsersWorkouts).HasForeignKey(x => x.WorkoutId);
            builder.HasOne(x => x.WorkoutStatusEntity).WithMany(x => x.UsersWorkouts).HasForeignKey(x => x.WorkoutStatus).OnDelete(DeleteBehavior.ClientSetNull);
            builder.ToTable("UserWorkout", "public");
        });
    }
}
