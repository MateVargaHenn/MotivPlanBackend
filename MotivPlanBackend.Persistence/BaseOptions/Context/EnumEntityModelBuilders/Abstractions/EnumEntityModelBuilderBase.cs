using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders.Abstractions;

internal abstract class EnumEntityModelBuilderBase
{
    protected void ConfigureCommonProperties<T, TEnumEntity>(EntityTypeBuilder<T> builder) where T : EnumEntity<TEnumEntity> where TEnumEntity : Enum
    {
        builder.Property(e => e.NormalizedName)
                .HasComputedColumnSql("upper(\"Name\")", stored: true)
                .IsRequired();
        builder.Property(e => e.ConcurrencyStamp)
                .HasDefaultValueSql("uuid_generate_v1()")
                .IsRequired();
    }
}
