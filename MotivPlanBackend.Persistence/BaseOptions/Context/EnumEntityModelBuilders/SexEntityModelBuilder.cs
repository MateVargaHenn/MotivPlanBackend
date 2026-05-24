using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders.Abstractions;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders;

internal sealed class SexEntityModelBuilder : EnumEntityModelBuilderBase
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SexEntity>(builder =>
        {
            ConfigureCommonProperties<SexEntity, Sex>(builder);
            builder.ToTable("Sex", "public");
        });
    }
}
