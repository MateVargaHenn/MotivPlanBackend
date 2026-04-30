using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal class WorkoutStatusEntityModelBuilder : EntityModelBuilderBase
{
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public void ConfigureModel(ModelBuilder modelBuilder)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
    {
        modelBuilder.Entity<WorkoutStatusEntity>(builder =>
        {
            builder.Property(e => e.NormalizedName)
                    .HasComputedColumnSql("upper(\"Name\")", stored: true)
                    .IsRequired();
            builder.Property(e => e.ConcurrencyStamp)
                    .HasDefaultValueSql("uuid_generate_v1()")
                    .IsRequired();
            builder.ToTable("WorkoutStatus", "public");
        });
    }
}