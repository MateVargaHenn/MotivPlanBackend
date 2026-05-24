using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

public class ProfilePreferenceEntityModelBuilder : IEntityTypeConfiguration<ProfilePreferenceEntity>
{
    public void Configure(EntityTypeBuilder<ProfilePreferenceEntity> builder)
    {
        builder.HasKey(e => new { e.ProfileId, e.PreferenceId });
        builder.HasOne(e => e.Profile)
            .WithMany(p => p.ProfilePreferences)
            .HasForeignKey(e => e.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Preference)
            .WithMany(p => p.ProfilePreferences)
            .HasForeignKey(e => e.PreferenceId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.ToTable("ProfilePreference");
    }
}
