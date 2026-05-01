using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal sealed class ExerciseEntityModelBuilder : EntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExerciseEntity>(builder =>
        {
            ConfigureCommonProperties(builder);
            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.Description)
                .HasMaxLength(2000);
            builder.ToTable("Exercises", "public");
        });
    }
}
