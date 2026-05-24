using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders;

internal sealed class PreferenceEntityModelBuilder : EnumEntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PreferenceEntity>(builder =>
        {
            ConfigureCommonProperties<PreferenceEntity, Preference>(builder);

                    builder.HasMany(e => e.ProfilePreferences)
            .WithOne(p => p.Preference)
            .HasForeignKey(e => e.PreferenceId)
            .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Preferences", "public");
        });
    }
}
