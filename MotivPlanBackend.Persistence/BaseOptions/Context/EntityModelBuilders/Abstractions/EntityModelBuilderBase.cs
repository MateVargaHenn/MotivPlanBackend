using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders.Abstractions;

internal abstract class EntityModelBuilderBase
{
    private readonly string _dateTimeType = "timestamp";

    protected void ConfigureCommonProperties<T>(EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.Property(e => e.LastModified)
            .HasColumnType(_dateTimeType)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System")
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}
