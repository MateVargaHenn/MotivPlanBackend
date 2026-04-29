using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal class WorkoutEntityModelBuilder : EntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkoutEntity>(builder =>
        {
            ConfigureCommonProperties(builder);
            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.ToTable("Workouts", "public");
        });
    }
}