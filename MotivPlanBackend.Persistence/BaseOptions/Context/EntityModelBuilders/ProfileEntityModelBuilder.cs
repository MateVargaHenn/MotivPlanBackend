using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal sealed class ProfileEntityModelBuilder : EntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileEntity>(builder =>
        {
            ConfigureCommonProperties(builder);
            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.BirthDate)
                .IsRequired();
            builder.Property(e => e.SexId)
                .IsRequired();
            builder.Property(e => e.Weight)
                .IsRequired();
            builder.Property(e => e.Height)
                .IsRequired();
            builder.HasMany(e => e.ProfilePreferences)
                .WithOne(p => p.Profile)
                .HasForeignKey(e => e.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Profiles", "public");
        });
    }
}
