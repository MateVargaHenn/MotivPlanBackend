using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders;

internal class WorkoutStatusEntityModelBuilder : EnumEntityModelBuilderBase
{
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public void ConfigureModel(ModelBuilder modelBuilder)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
    {
        modelBuilder.Entity<WorkoutStatusEntity>(builder =>
        {
            ConfigureCommonProperties<WorkoutStatusEntity, WorkoutStatus>(builder);
            builder.ToTable("WorkoutStatus", "public");
        });
    }
}